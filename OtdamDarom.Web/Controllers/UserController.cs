using OtdamDarom.BusinessLogic.Dtos;
using OtdamDarom.BusinessLogic.Interfaces;
using System;
using System.IO;
using System.Web.Mvc;
using OtdamDarom.Web.Filters;
using System.Security.Claims;
using System.Threading.Tasks; 

namespace OtdamDarom.Controllers
{
    [CustomAuthorize]
    public class UserController : Controller
    {
        private readonly IAuth _auth;
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
            };
        }
        
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
                userProfile.CurrentPassword = null;
                userProfile.NewPassword = null;
                userProfile.ConfirmNewPassword = null;
                return View("Profile", userProfile);
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
            
            bool isModelValid = ModelState.IsValid;
            
            var originalProfile = await GetUserProfileFromBL(currentUserId);
            if (originalProfile == null)
            {
                TempData["ErrorMessage"] = "Profilul utilizatorului nu a fost găsit.";
                return RedirectToAction("Profile");
            }
            
            model.UserRole = originalProfile.UserRole;
            model.CreationDate = originalProfile.CreationDate;

            bool passwordFieldsFilled = !string.IsNullOrEmpty(model.CurrentPassword) ||
                                        !string.IsNullOrEmpty(model.NewPassword) ||
                                        !string.IsNullOrEmpty(model.ConfirmNewPassword);

            if (passwordFieldsFilled)
            {

                ModelState.Remove("CurrentPassword");
                ModelState.Remove("NewPassword");
                ModelState.Remove("ConfirmNewPassword");
                
                if (string.IsNullOrEmpty(model.CurrentPassword))
                {
                    ModelState.AddModelError("CurrentPassword", "Parola actuală este obligatorie pentru a schimba parola.");
                }
                if (string.IsNullOrEmpty(model.NewPassword))
                {
                    ModelState.AddModelError("NewPassword", "Parola nouă este obligatorie.");
                }
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
                    bool passwordUpdateSuccess = await _user.UpdatePassword(currentUserId, model.CurrentPassword, model.NewPassword);
                    if (passwordUpdateSuccess)
                    {
                        TempData["SuccessMessage"] = "Parola a fost actualizată cu succes!";
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

            string newRelativePictureUrl = originalProfile.ProfilePictureUrl;

            if (model.NewProfilePicture != null && model.NewProfilePicture.ContentLength > 0)
            {
                if (!model.NewProfilePicture.ContentType.StartsWith("image/"))
                {
                    ModelState.AddModelError("NewProfilePicture", "Fișierul selectat nu este o imagine.");
                    isModelValid = false;
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
                        isModelValid = false;
                    }
                }
            }
            
            model.ProfilePictureUrl = newRelativePictureUrl;
            
            if (!ModelState.IsValid)
            {
                return View("Profile", model);
            }
            
            bool detailsUpdateSuccess = true;
            if (isModelValid)
            {
                detailsUpdateSuccess = await UpdateUserProfileDetails(model); 
                if (!detailsUpdateSuccess) 
                {
                    ModelState.AddModelError("", "A apărut o eroare la salvarea detaliilor profilului.");
                    return View("Profile", model);
                }
            }
            
            Session["Username"] = model.Name;
            Session["UserEmail"] = model.Email; 
            Session["UserProfilePicUrl"] = model.ProfilePictureUrl; 
            
            UserProfileDto updatedProfile = await GetUserProfileFromBL(currentUserId);
            TempData["SuccessMessage"] = TempData["SuccessMessage"] ?? "Profilul a fost actualizat cu succes!";

            return View("Profile", updatedProfile); 
        }
    } 
}