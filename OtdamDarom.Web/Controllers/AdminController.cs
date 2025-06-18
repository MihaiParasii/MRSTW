using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web; // Adăugat pentru HttpPostedFileBase
using System.Web.Mvc;
using OtdamDarom.Domain.Models;
using OtdamDarom.BusinessLogic.Interfaces;
using OtdamDarom.BusinessLogic.Api;
using System.Linq; // Adăugat pentru .ToList() și alte operații LINQ
using OtdamDarom.BusinessLogic.Dtos; // Adăugat pentru a folosi DealDto

namespace OtdamDarom.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly BusinessLogic.BusinessLogic _businessLogic;
        private readonly IUser _userBl;
        private readonly AdminApi _adminApi;

        public AdminController()
        {
            _businessLogic = new BusinessLogic.BusinessLogic();
            _userBl = _businessLogic.GetUserBL();
            _adminApi = new AdminApi();
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
            throw new UnauthorizedAccessException("ID-ul utilizatorului nu a putut fi determinat. Sesiunea poate fi expirată sau utilizatorul nu este autentificat.");
        }

        public async Task<ActionResult> Dashboard()
        {
            ViewBag.Title = "Dashboard Admin";
            ViewBag.TotalUsers = await _adminApi.GetTotalUsersCountAsync();
            ViewBag.TotalDeals = await _adminApi.GetTotalDealsCountAsync();
            ViewBag.LatestUsers = await _adminApi.GetLatestUsersAsync(5);
            ViewBag.LatestDeals = await _adminApi.GetLatestDealsAsync(5);
            return View();
        }

        public async Task<ActionResult> Users()
        {
            ViewBag.Title = "Gestionează Utilizatori";
            var users = await _userBl.GetAllUsers();
            return View(users);
        }

        public async Task<ActionResult> UserDetails(int id)
        {
            var user = await _userBl.GetUserById(id);
            if (user == null) { return HttpNotFound(); }
            return View(user);
        }

        public ActionResult CreateUser() { return View(new UserModel()); }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateUser(UserModel user)
        {
            if (ModelState.IsValid)
            {
                await _adminApi.CreateUserAsync(user);
                TempData["SuccessMessage"] = "Utilizatorul a fost creat cu succes!";
                return RedirectToAction("Users");
            }
            TempData["ErrorMessage"] = "Eroare la crearea utilizatorului. Verificați datele introduse.";
            return View(user);
        }

        [HttpGet] // Adăugat explicit HttpGet pentru claritate
        public async Task<ActionResult> EditUser(int id)
        {
            ViewBag.Title = "Editează Utilizatorul";
            var user = await _adminApi.GetUserByIdAsync(id);
            if (user == null) { return HttpNotFound(); }
            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditUser(UserModel user, HttpPostedFileBase imageFile, bool DeleteExistingImage = false)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _adminApi.UpdateUserAsync(user, imageFile, DeleteExistingImage);
                    TempData["SuccessMessage"] = "Utilizatorul a fost actualizat cu succes!";
                    return RedirectToAction("Dashboard");
                }
                catch (ArgumentException ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "A apărut o eroare la actualizarea utilizatorului: " + ex.Message);
                }
            }
            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> DeleteUser(int id)
        {
            try
            {
                await _adminApi.DeleteUserAsync(id);
                return Json(new { success = true, message = "Utilizatorul a fost șters cu succes." });
            }
            catch (ArgumentException ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
            catch (Exception)
            {
                return Json(new { success = false, message = "A apărut o eroare internă la ștergerea utilizatorului." });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> DeleteDeal(int id)
        {
            try
            {
                await _adminApi.DeleteDealAsync(id);
                return Json(new { success = true, message = "Anunțul a fost șters cu succes." });
            }
            catch (ArgumentException ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
            catch (Exception)
            {
                return Json(new { success = false, message = "A apărut o eroare internă la ștergerea anunțului." });
            }
        }

        public async Task<ActionResult> EditUserRole(string email)
        {
            ViewBag.Users = new SelectList(await _userBl.GetAllUsers(), "Email", "Email");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> UpdateUserRole(string email, string newRole)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(newRole))
            {
                TempData["ErrorMessage"] = "Emailul și rolul nou sunt obligatorii.";
                ViewBag.Users = new SelectList(await _userBl.GetAllUsers(), "Email", "Email");
                return View("EditUserRole");
            }
            await _userBl.UpdateUserRole(email, newRole);
            TempData["SuccessMessage"] = $"Rolul utilizatorului {email} a fost actualizat la {newRole} cu succes!";
            return RedirectToAction("Users");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteSelectedUsers(int[] selectedUserIds)
        {
            if (selectedUserIds != null && selectedUserIds.Length > 0)
            {
                foreach (var id in selectedUserIds)
                {
                    try
                    {
                        await _adminApi.DeleteUserAsync(id);
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine($"Eroare la ștergerea utilizatorului {id}: {ex.Message}");
                    }
                }
                TempData["SuccessMessage"] = $"{selectedUserIds.Length} utilizatori au fost șterși cu succes!";
            }
            else
            {
                TempData["ErrorMessage"] = "Niciun utilizator nu a fost selectat pentru ștergere.";
            }
            return RedirectToAction("Users");
        }

        public ActionResult ManageDeals()
        {
            ViewBag.Title = "Gestionează Anunțuri";
            return View();
        }

        // GET: Admin/EditDeal/5
        [HttpGet] // Adăugat explicit HttpGet pentru claritate
        public async Task<ActionResult> EditDeal(int id)
        {
            ViewBag.Title = "Editează Anunțul";
            var deal = await _adminApi.GetDealByIdAsync(id); // Obține DealModel
            if (deal == null)
            {
                return HttpNotFound();
            }

            // Mapează DealModel la DealDto (aici aveai problema "Cannot resolve symbol")
            var dealDto = new DealDto
            {
                Id = deal.Id,
                Name = deal.Name,
                Description = deal.Description,
                ImageURL = deal.ImageURL,
                UserId = deal.UserId, // Această linie nu va mai da eroare după modificarea DealDto
                CreationDate = deal.CreationDate, // Această linie nu va mai da eroare după modificarea DealDto
                SelectedCategoryId = deal.Subcategory?.Category?.Id,
                SelectedSubcategoryId = deal.Subcategory?.Id ?? 0 // Dacă SubcategoryId este int și poate fi 0, asigură-te că 0 este o valoare validă sau ajustează.
            };

            ViewBag.Categories = new SelectList(await _adminApi.GetAllCategoriesAsync(), "Id", "Name", dealDto.SelectedCategoryId);

            var subcategoriesForCurrentCategory = new List<SubcategoryModel>();
            if (dealDto.SelectedCategoryId.HasValue && dealDto.SelectedCategoryId.Value > 0)
            {
                subcategoriesForCurrentCategory = (await _adminApi.GetSubcategoriesByCategoryIdAsync(dealDto.SelectedCategoryId.Value))?.ToList();
            }
            ViewBag.Subcategories = new SelectList(subcategoriesForCurrentCategory, "Id", "Name", dealDto.SelectedSubcategoryId);

            return View(dealDto); // Trimite DTO-ul către view
        }

        // POST: Admin/EditDeal
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditDeal(DealDto dealDto) // Primește DealDto din formular
        {
            // Re-populează ViewBag-urile în caz de eroare de validare
            ViewBag.Categories = new SelectList(await _adminApi.GetAllCategoriesAsync(), "Id", "Name", dealDto.SelectedCategoryId);

            var subcategoriesForCurrentCategory = new List<SubcategoryModel>();
            if (dealDto.SelectedCategoryId.HasValue && dealDto.SelectedCategoryId.Value > 0)
            {
                subcategoriesForCurrentCategory = (await _adminApi.GetSubcategoriesByCategoryIdAsync(dealDto.SelectedCategoryId.Value))?.ToList();
            }
            ViewBag.Subcategories = new SelectList(subcategoriesForCurrentCategory, "Id", "Name", dealDto.SelectedSubcategoryId);

            // Validăm modelul DealDto înainte de a încerca salvarea în baza de date
            if (ModelState.IsValid)
            {
                try
                {
                    // Obținem anunțul existent din baza de date pentru a actualiza proprietățile
                    var existingDeal = await _adminApi.GetDealByIdAsync(dealDto.Id);
                    if (existingDeal == null)
                    {
                        ModelState.AddModelError("", "Anunțul nu a fost găsit pentru actualizare.");
                        return View(dealDto);
                    }

                    // Actualizăm doar proprietățile pe care utilizatorul le poate modifica prin DTO
                    existingDeal.Name = dealDto.Name;
                    existingDeal.Description = dealDto.Description;
                    existingDeal.SubcategoryId = dealDto.SelectedSubcategoryId;

                    // UserId și CreationDate sunt preluate din DTO (unde ar trebui să ajungă prin HiddenFor în View)
                    // Ele nu sunt modificate aici, doar re-confirmate.
                    // existingDeal.UserId = dealDto.UserId; // Nu este necesar să le atribui aici dacă nu le modifici
                    // existingDeal.CreationDate = dealDto.CreationDate; // Nu este necesar să le atribui aici

                    // Apelează AdminApi pentru a actualiza anunțul, inclusiv gestionarea imaginilor
                    await _adminApi.UpdateDealAsync(existingDeal, dealDto.ImageFile, dealDto.DeleteExistingImage);
                    
                    TempData["SuccessMessage"] = "Anunțul a fost actualizat cu succes!";
                    return RedirectToAction("Dashboard");
                }
                catch (ArgumentException ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
                catch (Exception ex)
                {
                    // Aceasta este eroarea "An error occurred while updating the entries."
                    // Prinde excepția și afișează un mesaj util.
                    System.Diagnostics.Debug.WriteLine($"Eroare detaliată la actualizarea anunțului: {ex.InnerException?.Message ?? ex.Message}");
                    ModelState.AddModelError("", "A apărut o eroare la actualizarea anunțului: " + ex.Message); // Sau ex.InnerException?.Message pentru mai multe detalii
                }
            }
            // Dacă ModelState nu este valid sau a apărut o eroare la salvare, returnează DTO-ul la view.
            return View(dealDto);
        }

        public async Task<JsonResult> GetSubcategories(int categoryId)
        {
            var subcategories = await _adminApi.GetSubcategoriesByCategoryIdAsync(categoryId);
            return Json(subcategories.Select(s => new { Id = s.Id, Name = s.Name }), JsonRequestBehavior.AllowGet);
        }

        public ActionResult ManageCategories()
        {
            ViewBag.Title = "Gestionează Categorii";
            return View();
        }
    }
}