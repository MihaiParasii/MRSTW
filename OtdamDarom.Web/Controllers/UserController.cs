using OtdamDarom.BusinessLogic.Dtos;
using OtdamDarom.BusinessLogic.Interfaces;
using System;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OtdamDarom.Web.Filters;
using System.Security.Claims;
using System.Threading.Tasks; 
using OtdamDarom.Domain.Models; 

namespace OtdamDarom.Controllers
{
    [CustomAuthorize]
    public class UserController : Controller
    {
        private readonly IAuth _auth; // Poate ai nevoie de AuthBL pentru alte operațiuni, o păstrăm
        private readonly IUser _user;

        public UserController()
        {
            var bl = new BusinessLogic.BusinessLogic(); 
            _auth = bl.GetAuthBL(); 
            _user = bl.GetUserBL(); 
        }

        private int GetCurrentUserId()
        {
            var identity = User.Identity as ClaimsIdentity;
            if (identity != null)
            {
                var userIdClaim = identity.FindFirst(ClaimTypes.NameIdentifier);
                if (userIdClaim != null && int.TryParse(userIdClaim.Value, out int userId))
                {
                    return userId;
                }
            }
            if (Session["UserId"] != null)
            {
                return (int)Session["UserId"];
            }
            throw new UnauthorizedAccessException("ID-ul utilizatorului nu a putut fi determinat. Sesiunea poate fi expirată.");
        }

        private async Task<UserProfileDto> GetUserProfileFromBL(int userId)
        {
            var userModel = await _user.GetUserById(userId); 
            
            if (userModel == null) return null;

            return new UserProfileDto
            {
                Id = userModel.Id,
                Name = userModel.Name,
                Email = userModel.Email,
                UserRole = userModel.UserRole,
                ProfilePictureUrl = userModel.ProfilePictureUrl, 
                CreationDate = userModel.CreationDate
                // Nu încărcăm câmpurile de parolă aici pentru GET
            };
        }

        // Metodă helper pentru actualizarea profilului (fără parolă)
        private async Task<bool> UpdateUserProfileDetails(UserProfileDto model)
        {
            var userModel = await _user.GetUserById(model.Id); 
            if (userModel == null) return false;

            userModel.Name = model.Name;
            userModel.Email = model.Email;
            userModel.ProfilePictureUrl = model.ProfilePictureUrl; 

            try
            {
                await _user.UpdateUser(userModel); 
                return true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Eroare la actualizarea profilului (detalii): {ex.Message}");
                return false;
            }
        }

        // GET: User/Profile
        public async Task<ActionResult> Profile()
        {
            try
            {
                int userId = GetCurrentUserId();
                UserProfileDto userProfile = await GetUserProfileFromBL(userId);
                
                if (userProfile == null)
                {
                    TempData["ErrorMessage"] = "Profilul utilizatorului nu a fost găsit.";
                    return RedirectToAction("Index", "Home"); 
                }
                // Nu populăm câmpurile de parolă pentru GET
                userProfile.CurrentPassword = null;
                userProfile.NewPassword = null;
                userProfile.ConfirmNewPassword = null;

                return View(userProfile);
            }
            catch (UnauthorizedAccessException)
            {
                TempData["ErrorMessage"] = "Sesiunea a expirat. Te rugăm să te autentifici din nou.";
                return RedirectToAction("Login", "Auth");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Eroare la încărcarea profilului: {ex.Message}");
                TempData["ErrorMessage"] = "A apărut o eroare la încărcarea profilului.";
                return RedirectToAction("Index", "Home");
            }
        }

        // GET: User/EditProfile (va folosi aceeași vizualizare Profile)
        public async Task<ActionResult> EditProfile()
        {
            try
            {
                int userId = GetCurrentUserId(); 
                UserProfileDto userProfile = await GetUserProfileFromBL(userId);
                
                if (userProfile == null)
                {
                    TempData["ErrorMessage"] = "Profilul utilizatorului nu a fost găsit pentru editare.";
                    return RedirectToAction("Profile");
                }
                ViewBag.Title = "Editează Profilul"; 
                // Nu populăm câmpurile de parolă pentru GET
                userProfile.CurrentPassword = null;
                userProfile.NewPassword = null;
                userProfile.ConfirmNewPassword = null;
                return View("Profile", userProfile); // Returnează aceeași vizualizare "Profile"
            }
            catch (UnauthorizedAccessException)
            {
                TempData["ErrorMessage"] = "Sesiunea a expirat. Te rugăm să te autentifici din nou.";
                return RedirectToAction("Login", "Auth");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Eroare la încărcarea paginii de editare profil: {ex.Message}");
                TempData["ErrorMessage"] = "A apărut o eroare la încărcarea paginii de editare profil.";
                return RedirectToAction("Profile");
            }
        }

        // POST: User/EditProfile
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditProfile(UserProfileDto model)
        {
            ViewBag.Title = "Editează Profilul";
            int currentUserId = GetCurrentUserId();

            if (model.Id != currentUserId)
            {
                TempData["ErrorMessage"] = "Nu ești autorizat să editezi acest profil.";
                return RedirectToAction("Profile");
            }

            // Păstrăm un ModelState.IsValid inițial pentru validările de bază (Name, Email)
            // dar vom face validări specifice pentru parolă mai jos.
            bool isModelValid = ModelState.IsValid; // Verifică Name, Email, ProfilePictureUrl (dacă e cazul)

            // Reîncarcă modelul original pentru a păstra datele non-editabile și a le afișa corect în caz de eroare
            // sau pentru a asigura că proprietățile nu sunt null dacă nu au fost trimise de client (ex: UserRole, CreationDate)
            var originalProfile = await GetUserProfileFromBL(currentUserId);
            if (originalProfile == null)
            {
                TempData["ErrorMessage"] = "Profilul utilizatorului nu a fost găsit.";
                return RedirectToAction("Profile");
            }

            // Copiază proprietățile nemodificabile înapoi în modelul trimis pentru afișare corectă în View
            model.UserRole = originalProfile.UserRole;
            model.CreationDate = originalProfile.CreationDate;
            // ProfilePictureUrl va fi gestionat separat mai jos

            // --- Logica pentru schimbarea parolei ---
            bool passwordFieldsFilled = !string.IsNullOrEmpty(model.CurrentPassword) ||
                                        !string.IsNullOrEmpty(model.NewPassword) ||
                                        !string.IsNullOrEmpty(model.ConfirmNewPassword);

            if (passwordFieldsFilled)
            {
                // Validăm câmpurile de parolă doar dacă cel puțin unul a fost completat
                // Clear validation errors for password fields initially, then re-validate specifically
                ModelState.Remove("CurrentPassword");
                ModelState.Remove("NewPassword");
                ModelState.Remove("ConfirmNewPassword");

                // Re-adaugăm validarea pentru câmpurile de parolă
                if (string.IsNullOrEmpty(model.CurrentPassword))
                {
                    ModelState.AddModelError("CurrentPassword", "Parola actuală este obligatorie pentru a schimba parola.");
                }
                if (string.IsNullOrEmpty(model.NewPassword))
                {
                    ModelState.AddModelError("NewPassword", "Parola nouă este obligatorie.");
                }
                // Validarea [Compare] este automată, nu trebuie să o faci manual aici
                if (!string.IsNullOrEmpty(model.NewPassword) && !string.IsNullOrEmpty(model.ConfirmNewPassword) && model.NewPassword != model.ConfirmNewPassword)
                {
                     ModelState.AddModelError("ConfirmNewPassword", "Parola nouă și confirmarea parolei nu se potrivesc.");
                }
                if (!string.IsNullOrEmpty(model.NewPassword) && model.NewPassword.Length < 6)
                {
                    ModelState.AddModelError("NewPassword", "Parola nouă trebuie să aibă cel puțin 6 caractere.");
                }
                
                if (ModelState.IsValidField("CurrentPassword") && ModelState.IsValidField("NewPassword") && ModelState.IsValidField("ConfirmNewPassword"))
                {
                    // Încercăm să actualizăm parola prin Business Logic
                    bool passwordUpdateSuccess = await _user.UpdatePassword(currentUserId, model.CurrentPassword, model.NewPassword);
                    if (passwordUpdateSuccess)
                    {
                        TempData["SuccessMessage"] = "Parola a fost actualizată cu succes!";
                        // Clear password fields after successful update
                        model.CurrentPassword = null;
                        model.NewPassword = null;
                        model.ConfirmNewPassword = null;
                    }
                    else
                    {
                        ModelState.AddModelError("", "Eroare la schimbarea parolei. Asigură-te că parola actuală este corectă.");
                    }
                }
            }


            // --- Logica pentru actualizarea detaliilor de profil (Nume, Email, Imagine) ---
            // Această logică se execută chiar dacă se încearcă doar o schimbare de parolă,
            // dar va avea efect doar dacă datele sunt modificate.

            string newRelativePictureUrl = originalProfile.ProfilePictureUrl; // Pornim de la URL-ul existent

            if (model.NewProfilePicture != null && model.NewProfilePicture.ContentLength > 0)
            {
                if (!model.NewProfilePicture.ContentType.StartsWith("image/"))
                {
                    ModelState.AddModelError("NewProfilePicture", "Fișierul selectat nu este o imagine.");
                    isModelValid = false; // Marchează modelul ca nevalid pentru detalii de profil
                }
                else
                {
                    string uploadsFolder = Server.MapPath("~/Content/Images"); 
                    if (!Directory.Exists(uploadsFolder))
                    {
                        Directory.CreateDirectory(uploadsFolder);
                    }

                    string fileExtension = Path.GetExtension(model.NewProfilePicture.FileName);
                    string uniqueFileName = Guid.NewGuid().ToString() + fileExtension;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    try
                    {
                        model.NewProfilePicture.SaveAs(filePath);
                        newRelativePictureUrl = Url.Content("~/Content/Images/" + uniqueFileName);
                        
                        string oldProfilePictureUrl = originalProfile.ProfilePictureUrl; 
                        
                        if (!string.IsNullOrEmpty(oldProfilePictureUrl) && 
                            !oldProfilePictureUrl.Contains("default-user.png") && 
                            !oldProfilePictureUrl.Equals(newRelativePictureUrl, StringComparison.OrdinalIgnoreCase))
                        {
                            string oldPhysicalPath = Server.MapPath(oldProfilePictureUrl);
                            if (System.IO.File.Exists(oldPhysicalPath))
                            {
                                System.IO.File.Delete(oldPhysicalPath);
                                System.Diagnostics.Debug.WriteLine($"Vechea imagine {oldPhysicalPath} a fost ștearsă.");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError("NewProfilePicture", $"Eroare la salvarea imaginii: {ex.Message}");
                        System.Diagnostics.Debug.WriteLine($"Eroare salvare imagine profil: {ex.Message}");
                        isModelValid = false; // Marchează modelul ca nevalid
                    }
                }
            }

            // Setează URL-ul final al imaginii în model (pentru actualizarea BL și pentru a-l pasa înapoi în View)
            model.ProfilePictureUrl = newRelativePictureUrl;


            // Verifică dacă există erori de validare pentru detalii de profil (Name, Email, NewProfilePicture)
            // sau dacă există erori de validare pentru parolă
            if (!ModelState.IsValid) // ModelState.IsValid va fi false dacă s-au adăugat erori pentru parolă sau alte câmpuri
            {
                 // Dacă modelul nu este valid (după toate validările), reîntoarce view-ul cu erori
                return View("Profile", model);
            }

            // Încercăm să actualizăm detaliile profilului doar dacă nu au fost erori la imagine/nume/email
            bool detailsUpdateSuccess = true;
            if (isModelValid) // Dacă validările inițiale (Name, Email, NewProfilePicture) au trecut
            {
                detailsUpdateSuccess = await UpdateUserProfileDetails(model); 
                if (!detailsUpdateSuccess) 
                {
                    ModelState.AddModelError("", "A apărut o eroare la salvarea detaliilor profilului.");
                    return View("Profile", model);
                }
            }
            
            // Actualizează datele în sesiune pentru a se reflecta imediat modificările în UI
            Session["Username"] = model.Name;
            Session["UserEmail"] = model.Email; 
            Session["UserProfilePicUrl"] = model.ProfilePictureUrl; 

            // Reîncărcăm profilul după actualizare pentru a asigura consistența datelor afișate
            UserProfileDto updatedProfile = await GetUserProfileFromBL(currentUserId);
            TempData["SuccessMessage"] = TempData["SuccessMessage"] ?? "Profilul a fost actualizat cu succes!"; // Păstrează mesajul de la parolă sau setează un nou mesaj

            return View("Profile", updatedProfile); 
        } // <-- Aici se închide corect metoda EditProfile (POST)
    } // <-- Aici se închide clasa UserController
} // <-- Aici se închide namespace-ul OtdamDarom.Controllers