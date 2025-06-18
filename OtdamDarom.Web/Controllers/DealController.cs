using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web; // Adaugă pentru HttpPostedFileBase
using System.Web.Mvc;
using System.Security.Claims; // Adaugă pentru ClaimsIdentity
using AutoMapper;
using OtdamDarom.BusinessLogic.Interfaces;
using OtdamDarom.BusinessLogic.Dtos; // DTO pentru anunț (DealDto)
using OtdamDarom.Domain.Models; // Pentru DealModel
using OtdamDarom.Web.Filters; // Pentru [CustomAuthorize]
using OtdamDarom.Web.Requests; // Asigură-te că DealResponse este aici

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

                // <<<<<<<<<<<<<<<<< CORECTAT AICI >>>>>>>>>>>>>>>>>>>>>>
                // Aici era eroarea. dealModel.SubcategoryId este int?, deci trebuie verificat HasValue și folosit .Value
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
                // <<<<<<<<<<<<<<<<< SFÂRȘIT CORECTAT >>>>>>>>>>>>>>>>>>>>>>
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

        // GET: Deal/Create - Afișează formularul pentru adăugarea unui anunț nou
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

        // POST: Deal/Create - Procesează trimiterea formularului unui anunț nou
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
                        // <<<<<<<<<<<<<<<<< CORECTAT AICI >>>>>>>>>>>>>>>>>>>>>>
                        // La creare, SelectedSubcategoryId este int (non-nullable conform DTO)
                        // SubcategoryId în DealModel este int? (nullable)
                        // Conversia de la int la int? este implicită și sigură.
                        SubcategoryId = model.SelectedSubcategoryId, 
                        // <<<<<<<<<<<<<<<<< SFÂRȘIT CORECTAT >>>>>>>>>>>>>>>>>>>>>>
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

        // GET: Deal/Edit - Afișează formularul pentru editarea unui anunț existent
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

                // Verifică dacă utilizatorul curent este proprietarul anunțului
                if (dealModel.UserId != currentUserId)
                {
                    TempData["ErrorMessage"] = "Nu ai permisiunea de a edita acest anunț.";
                    return RedirectToAction("MyDeals");
                }

                // Încarcă toate categoriile și subcategoriile
                var categories = await _userBl.GetAllCategoriesWithSubcategories();
                
                // Selectează categoria și subcategoria curentă a anunțului
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
                
                // Încarcă subcategoriile pentru categoria selectată a anunțului
                if (currentCategoryId.HasValue)
                {
                    var currentCategory = categories.FirstOrDefault(c => c.Id == currentCategoryId.Value);
                    if (currentCategory != null && currentCategory.Subcategories != null)
                    {
                        // <<<<<<<<<<<<<<<<< CORECTAT AICI >>>>>>>>>>>>>>>>>>>>>>
                        // Aici era eroarea. dealModel.SubcategoryId este int?, deci trebuie folosit .Value dacă are.
                        // Dar DropDownListFor acceptă int? pentru valoarea selectată, deci e ok să-l pasezi direct.
                        // Totuși, este mai explicit să verifici dacă are valoare și să o pasezi.
                        ViewBag.Subcategories = new SelectList(currentCategory.Subcategories, "Id", "Name", dealModel.SubcategoryId);
                        // <<<<<<<<<<<<<<<<< SFÂRȘIT CORECTAT >>>>>>>>>>>>>>>>>>>>>>
                    }
                }
                else
                {
                    ViewBag.Subcategories = new SelectList(new List<object>(), "Id", "Name"); // Lista goală dacă nu e categorie selectată
                }
                
                // Mapează DealModel la DealDto pentru formular
                var model = Mapper.Map<DealDto>(dealModel);
                model.SelectedCategoryId = currentCategoryId; // Setează ID-ul categoriei pentru a pre-selecta în dropdown

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

        // POST: Deal/Edit - Procesează trimiterea formularului de editare a anunțului
        [CustomAuthorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(DealDto model, HttpPostedFileBase imageFile)
        {
            // Reîncarcă categoriile și subcategoriile în caz de eroare de validare
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

                    // Verifică dacă utilizatorul curent este proprietarul anunțului
                    if (existingDeal.UserId != userId)
                    {
                        TempData["ErrorMessage"] = "Nu ai permisiunea de a edita acest anunț.";
                        return RedirectToAction("MyDeals");
                    }

                    string imageUrl = existingDeal.ImageURL; // Păstrează imaginea existentă dacă nu se încarcă una nouă

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

                        // Șterge imaginea veche dacă nu este cea implicită și dacă o nouă imagine este încărcată
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
                    else if (model.DeleteExistingImage) // Dacă utilizatorul a bifat ștergerea imaginii
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
                        imageUrl = "/Content/Images/default-deal.png"; // Setează imaginea implicită
                    }


                    existingDeal.Name = model.Name;
                    existingDeal.Description = model.Description;
                    existingDeal.ImageURL = imageUrl; // Actualizează URL-ul imaginii
                    // <<<<<<<<<<<<<<<<< CORECTAT AICI >>>>>>>>>>>>>>>>>>>>>>
                    // model.SelectedSubcategoryId este int, existingDeal.SubcategoryId este int?
                    // Conversia de la int la int? este implicită și sigură.
                    existingDeal.SubcategoryId = model.SelectedSubcategoryId; 
                    // <<<<<<<<<<<<<<<<< SFÂRȘIT CORECTAT >>>>>>>>>>>>>>>>>>>>>>
                    // CreationDate rămâne neschimbată
                    // UserId rămâne neschimbat

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


        // GET: Deal/GetSubcategories - Acțiune AJAX pentru a prelua subcategoriile unei categorii
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

        // NOU: GET: Deal/MyDeals - Afișează anunțurile utilizatorului logat
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
    }
}