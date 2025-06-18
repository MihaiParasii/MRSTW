using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Security.Claims;
using AutoMapper;
using OtdamDarom.BusinessLogic.Interfaces;
using OtdamDarom.BusinessLogic.Dtos;
using OtdamDarom.Domain.Models;
using OtdamDarom.Web.Filters;
using OtdamDarom.Web.Requests;

namespace OtdamDarom.Web.Controllers
{
    public class DealController : Controller
    {
        private readonly IDeal _dealBl;
        private readonly IUser _userBl;

        public DealController()
        {
            var bl = new BusinessLogic.BusinessLogic();
            _dealBl = bl.GetDealBL();
            _userBl = bl.GetUserBL();
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

        public async Task<ActionResult> Index()
        {
            IEnumerable<DealResponse> recentDeals = new List<DealResponse>();

            try
            {
                var deals = await _dealBl.GetAll().ConfigureAwait(false);

                if (deals != null && deals.Any())
                {
                    recentDeals = deals
                        .OrderByDescending(d => d.CreationDate)
                        .Take(12)
                        .Select(Mapper.Map<DealResponse>)
                        .ToList();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Eroare la obținerea anunțurilor principale: {ex.Message}");
                if (ex.InnerException != null)
                {
                    System.Diagnostics.Debug.WriteLine($"Detalii: {ex.InnerException.Message}");
                }
                recentDeals = new List<DealResponse>();
            }

            return View(recentDeals);
        }
        
        public async Task<ActionResult> Details(int id)
        {
            DealResponse dealResponse = null;
            IEnumerable<DealResponse> similarDeals = new List<DealResponse>();

            try
            {
                var dealModel = await _dealBl.GetById(id);

                if (dealModel == null)
                {
                    return HttpNotFound($"Anunțul cu ID-ul {id} nu a fost găsit.");
                }

                dealResponse = Mapper.Map<DealResponse>(dealModel);
                
                if (dealModel.SubcategoryId.HasValue && dealModel.SubcategoryId.Value > 0) 
                {
                    var dealsFromSameSubcategory = await _dealBl.GetDealsBySubcategoryIds(new List<int> { dealModel.SubcategoryId.Value });

                    if (dealsFromSameSubcategory != null && dealsFromSameSubcategory.Any())
                    {
                        similarDeals = dealsFromSameSubcategory
                                        .Where(d => d.Id != dealModel.Id)
                                        .OrderByDescending(d => d.CreationDate)
                                        .Take(4)
                                        .Select(Mapper.Map<DealResponse>)
                                        .ToList();
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Eroare la obținerea detaliilor anunțului sau anunțurilor similare pentru ID {id}: {ex.Message}");
                if (ex.InnerException != null)
                {
                    System.Diagnostics.Debug.WriteLine($"Detalii: {ex.InnerException.Message}");
                }
                return View("Error");
            }

            ViewBag.SimilarDeals = similarDeals;
            return View(dealResponse);
        }
        
        [CustomAuthorize]
        public async Task<ActionResult> Create()
        {
            try
            {
                var categories = await _userBl.GetAllCategoriesWithSubcategories();
                
                ViewBag.Categories = new SelectList(categories, "Id", "Name");
                
                var model = new DealDto();
                return View(model);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "A apărut o eroare la încărcarea formularului de anunț: " + ex.Message;
                System.Diagnostics.Debug.WriteLine($"Eroare în DealController.Create (GET): {ex.Message}");
                if (ex.InnerException != null) System.Diagnostics.Debug.WriteLine($"Inner Exception: {ex.InnerException.Message}");
                return RedirectToAction("Index", "Home");
            }
        }
        
        [CustomAuthorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(DealDto model, HttpPostedFileBase imageFile)
        {
            var categories = await _userBl.GetAllCategoriesWithSubcategories();
            ViewBag.Categories = new SelectList(categories, "Id", "Name", model.SelectedCategoryId);

            if (ModelState.IsValid)
            {
                try
                {
                    int userId = GetCurrentUserId();

                    string imageUrl = "/Content/Images/default-deal.png";
                    if (imageFile != null && imageFile.ContentLength > 0)
                    {
                        if (!imageFile.ContentType.StartsWith("image/"))
                        {
                            ModelState.AddModelError("ImageFile", "Fișierul selectat nu este o imagine.");
                            return View(model);
                        }
                        
                        const int maxFileSize = 5 * 1024 * 1024;
                        if (imageFile.ContentLength > maxFileSize)
                        {
                            ModelState.AddModelError("ImageFile", "Dimensiunea imaginii depășește limita de 5MB.");
                            return View(model);
                        }

                        string uploadsFolder = Server.MapPath("~/Content/DealImages");
                        if (!System.IO.Directory.Exists(uploadsFolder))
                        {
                            System.IO.Directory.CreateDirectory(uploadsFolder);
                        }

                        string fileExtension = System.IO.Path.GetExtension(imageFile.FileName);
                        string uniqueFileName = Guid.NewGuid().ToString() + fileExtension;
                        string filePath = System.IO.Path.Combine(uploadsFolder, uniqueFileName);

                        imageFile.SaveAs(filePath);
                        imageUrl = Url.Content("~/Content/DealImages/" + uniqueFileName);
                    }

                    var dealModel = new DealModel
                    {
                        Name = model.Name,
                        Description = model.Description,
                        ImageURL = imageUrl,
                        UserId = userId,
                        SubcategoryId = model.SelectedSubcategoryId, 
                        CreationDate = DateTime.Now
                    };

                    await _dealBl.Create(dealModel);

                    TempData["SuccessMessage"] = "Anunțul a fost adăugat cu succes!";
                    return RedirectToAction("Index", "Home");
                }
                catch (UnauthorizedAccessException)
                {
                    TempData["ErrorMessage"] = "Sesiunea a expirat. Te rugăm să te autentifici din nou.";
                    return RedirectToAction("Login", "Auth");
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Eroare la adăugarea anunțului: {ex.Message}");
                    if (ex.InnerException != null) System.Diagnostics.Debug.WriteLine($"Inner Exception: {ex.InnerException.Message}");
                    ModelState.AddModelError("", "A apărut o eroare la adăugarea anunțului: " + ex.Message);
                }
            }

            return View(model);
        }
        
        [CustomAuthorize]
        public async Task<ActionResult> Edit(int id)
        {
            try
            {
                int currentUserId = GetCurrentUserId();
                var dealModel = await _dealBl.GetById(id);

                if (dealModel == null)
                {
                    TempData["ErrorMessage"] = "Anunțul nu a fost găsit.";
                    return RedirectToAction("MyDeals");
                }
                
                if (dealModel.UserId != currentUserId)
                {
                    TempData["ErrorMessage"] = "Nu ai permisiunea de a edita acest anunț.";
                    return RedirectToAction("MyDeals");
                }
                
                var categories = await _userBl.GetAllCategoriesWithSubcategories();
                
                int? currentCategoryId = null;
                if (dealModel.SubcategoryId.HasValue)
                {
                    var subcategory = categories.SelectMany(c => c.Subcategories).FirstOrDefault(s => s.Id == dealModel.SubcategoryId.Value);
                    if (subcategory != null)
                    {
                        currentCategoryId = subcategory.CategoryId;
                    }
                }

                ViewBag.Categories = new SelectList(categories, "Id", "Name", currentCategoryId);
                
                if (currentCategoryId.HasValue)
                {
                    var currentCategory = categories.FirstOrDefault(c => c.Id == currentCategoryId.Value);
                    if (currentCategory != null && currentCategory.Subcategories != null)
                    {
                        ViewBag.Subcategories = new SelectList(currentCategory.Subcategories, "Id", "Name", dealModel.SubcategoryId);
                    }
                }
                else
                {
                    ViewBag.Subcategories = new SelectList(new List<object>(), "Id", "Name");
                }
                
                var model = Mapper.Map<DealDto>(dealModel);
                model.SelectedCategoryId = currentCategoryId;

                return View(model);
            }
            catch (UnauthorizedAccessException)
            {
                TempData["ErrorMessage"] = "Sesiunea a expirat. Te rugăm să te autentifici din nou.";
                return RedirectToAction("Login", "Auth");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Eroare în DealController.Edit (GET) pentru ID {id}: {ex.Message}");
                if (ex.InnerException != null) System.Diagnostics.Debug.WriteLine($"Inner Exception: {ex.InnerException.Message}");
                TempData["ErrorMessage"] = "A apărut o eroare la încărcarea formularului de editare a anunțului.";
                return RedirectToAction("MyDeals");
            }
        }
        
        [CustomAuthorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(DealDto model, HttpPostedFileBase imageFile)
        {
            var categories = await _userBl.GetAllCategoriesWithSubcategories();
            ViewBag.Categories = new SelectList(categories, "Id", "Name", model.SelectedCategoryId);

            if (model.SelectedCategoryId.HasValue)
            {
                var currentCategory = categories.FirstOrDefault(c => c.Id == model.SelectedCategoryId.Value);
                if (currentCategory != null && currentCategory.Subcategories != null)
                {
                    ViewBag.Subcategories = new SelectList(currentCategory.Subcategories, "Id", "Name", model.SelectedSubcategoryId);
                }
            }
            else
            {
                ViewBag.Subcategories = new SelectList(new List<object>(), "Id", "Name");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    int userId = GetCurrentUserId();
                    var existingDeal = await _dealBl.GetById(model.Id);

                    if (existingDeal == null)
                    {
                        TempData["ErrorMessage"] = "Anunțul nu a fost găsit pentru actualizare.";
                        return RedirectToAction("MyDeals");
                    }
                    
                    if (existingDeal.UserId != userId)
                    {
                        TempData["ErrorMessage"] = "Nu ai permisiunea de a edita acest anunț.";
                        return RedirectToAction("MyDeals");
                    }

                    string imageUrl = existingDeal.ImageURL;

                    if (imageFile != null && imageFile.ContentLength > 0)
                    {
                        if (!imageFile.ContentType.StartsWith("image/"))
                        {
                            ModelState.AddModelError("ImageFile", "Fișierul selectat nu este o imagine.");
                            return View(model);
                        }
                        
                        const int maxFileSize = 5 * 1024 * 1024;
                        if (imageFile.ContentLength > maxFileSize)
                        {
                            ModelState.AddModelError("ImageFile", "Dimensiunea imaginii depășește limita de 5MB.");
                            return View(model);
                        }
                        
                        if (!string.IsNullOrEmpty(existingDeal.ImageURL) && 
                            !existingDeal.ImageURL.Contains("default-deal.png"))
                        {
                            string oldFilePath = Server.MapPath(existingDeal.ImageURL);
                            if (System.IO.File.Exists(oldFilePath))
                            {
                                System.IO.File.Delete(oldFilePath);
                            }
                        }

                        string uploadsFolder = Server.MapPath("~/Content/DealImages");
                        if (!System.IO.Directory.Exists(uploadsFolder))
                        {
                            System.IO.Directory.CreateDirectory(uploadsFolder);
                        }

                        string fileExtension = System.IO.Path.GetExtension(imageFile.FileName);
                        string uniqueFileName = Guid.NewGuid().ToString() + fileExtension;
                        string filePath = System.IO.Path.Combine(uploadsFolder, uniqueFileName);

                        imageFile.SaveAs(filePath);
                        imageUrl = Url.Content("~/Content/DealImages/" + uniqueFileName);
                    }
                    else if (model.DeleteExistingImage)
                    {
                        if (!string.IsNullOrEmpty(existingDeal.ImageURL) &&
                            !existingDeal.ImageURL.Contains("default-deal.png"))
                        {
                            string oldFilePath = Server.MapPath(existingDeal.ImageURL);
                            if (System.IO.File.Exists(oldFilePath))
                            {
                                System.IO.File.Delete(oldFilePath);
                            }
                        }
                        imageUrl = "/Content/Images/default-deal.png";
                    }


                    existingDeal.Name = model.Name;
                    existingDeal.Description = model.Description;
                    existingDeal.ImageURL = imageUrl;
                    existingDeal.SubcategoryId = model.SelectedSubcategoryId; 
                    
                    await _dealBl.Update(existingDeal);

                    TempData["SuccessMessage"] = "Anunțul a fost actualizat cu succes!";
                    return RedirectToAction("MyDeals");
                }
                catch (UnauthorizedAccessException)
                {
                    TempData["ErrorMessage"] = "Sesiunea a expirat. Te rugăm să te autentifici din nou.";
                    return RedirectToAction("Login", "Auth");
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Eroare la actualizarea anunțului cu ID {model.Id}: {ex.Message}");
                    if (ex.InnerException != null) System.Diagnostics.Debug.WriteLine($"Inner Exception: {ex.InnerException.Message}");
                    ModelState.AddModelError("", "A apărut o eroare la actualizarea anunțului: " + ex.Message);
                }
            }

            return View(model);
        }
        
        [HttpGet]
        public async Task<JsonResult> GetSubcategories(int categoryId)
        {
            try
            {
                var category = await _userBl.GetCategoryById(categoryId); 
                if (category != null && category.Subcategories != null)
                {
                    var subcategories = category.Subcategories
                                                .Select(s => new { s.Id, s.Name })
                                                .ToList();
                    return Json(subcategories, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Eroare în GetSubcategories pentru CategoryId {categoryId}: {ex.Message}");
                if (ex.InnerException != null) System.Diagnostics.Debug.WriteLine($"Inner Exception: {ex.InnerException.Message}");
            }
            return Json(new List<object>(), JsonRequestBehavior.AllowGet);
        }
        
        [CustomAuthorize]
        public async Task<ActionResult> MyDeals()
        {
            IEnumerable<DealResponse> userDeals = new List<DealResponse>();
            try
            {
                int userId = GetCurrentUserId();
                var deals = await _dealBl.GetDealsByUserId(userId);

                if (deals != null && deals.Any())
                {
                    userDeals = deals.Select(Mapper.Map<DealResponse>).ToList();
                }
            }
            catch (UnauthorizedAccessException)
            {
                TempData["ErrorMessage"] = "Sesiunea a expirat. Te rugăm să te autentifici din nou.";
                return RedirectToAction("Login", "Auth");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Eroare la preluarea anunțurilor utilizatorului: {ex.Message}");
                if (ex.InnerException != null)
                {
                    System.Diagnostics.Debug.WriteLine($"Detalii: {ex.InnerException.Message}");
                }
                TempData["ErrorMessage"] = "A apărut o eroare la preluarea anunțurilor tale.";
                userDeals = new List<DealResponse>();
            }

            return View(userDeals);
        }
        
        [CustomAuthorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                int userId = GetCurrentUserId();
                var dealToDelete = await _dealBl.GetById(id);

                if (dealToDelete == null)
                {
                    return Json(new { success = false, message = "Anunțul nu a fost găsit." });
                }
                
                if (dealToDelete.UserId != userId)
                {
                    return Json(new { success = false, message = "Nu ai permisiunea de a șterge acest anunț." });
                }
                
                if (!string.IsNullOrEmpty(dealToDelete.ImageURL) && 
                    !dealToDelete.ImageURL.Contains("default-deal.png")) 
                {
                    string filePath = Server.MapPath(dealToDelete.ImageURL);
                    if (System.IO.File.Exists(filePath))
                    {
                        System.IO.File.Delete(filePath);
                    }
                }

                await _dealBl.Delete(id); 

                return Json(new { success = true, message = "Anunțul a fost șters cu succes!" });
            }
            catch (UnauthorizedAccessException)
            {
                return Json(new { success = false, message = "Sesiunea a expirat. Te rugăm să te autentifici din nou.", redirectTo = Url.Action("Login", "Auth") });
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Eroare la ștergerea anunțului cu ID {id}: {ex.Message}");
                if (ex.InnerException != null)
                {
                    System.Diagnostics.Debug.WriteLine($"Detalii: {ex.InnerException.Message}");
                }
                return Json(new { success = false, message = "A apărut o eroare la ștergerea anunțului. Vă rugăm să reîncărcați pagina și să încercați din nou." });
            }
        }
    }
}