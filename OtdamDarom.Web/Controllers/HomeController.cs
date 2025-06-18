using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using AutoMapper;
using OtdamDarom.BusinessLogic.Interfaces;
using OtdamDarom.Web.Requests;

namespace OtdamDarom.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IDeal _deal;
        private readonly ICategory _category;

        public HomeController()
        {
            var bl = new OtdamDarom.BusinessLogic.BusinessLogic();
            _deal = bl.GetDealBL();
            _category = bl.GetCategoryBL();
        }

        [AllowAnonymous]
        public async Task<ActionResult> Index()
        {
            IEnumerable<DealResponse> recentDeals = new List<DealResponse>();

            try
            {
                var deals = await _deal.GetAll().ConfigureAwait(false);

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
                System.Diagnostics.Debug.WriteLine($"Eroare la obținerea anunțurilor: {ex.Message}");
                if (ex.InnerException != null)
                {
                    System.Diagnostics.Debug.WriteLine($"Detalii: {ex.InnerException.Message}");
                }
                recentDeals = new List<DealResponse>();
            }

            return View(recentDeals);
        }

        [ChildActionOnly]
        [AllowAnonymous]
        public ActionResult _Sidebar()
        {
            IEnumerable<CategoryResponse> categoryResponses = new List<CategoryResponse>();
            try
            {
                var categories = _category.GetAllCategoriesWithSubcategories();
                if (categories != null && categories.Any())
                {
                    categoryResponses = Mapper.Map<IEnumerable<CategoryResponse>>(categories);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Eroare la obținerea categoriilor pentru sidebar: {ex.Message}");
                if (ex.InnerException != null)
                {
                    System.Diagnostics.Debug.WriteLine($"Detalii: {ex.InnerException.Message}");
                }
                categoryResponses = new List<CategoryResponse>();
            }

            return PartialView("_Sidebar", categoryResponses);
        }
        
        public async Task<ActionResult> DealsByCategory(int categoryId)
        {
            try
            {
                var selectedCategory = await _category.GetCategoryById(categoryId);

                if (selectedCategory == null)
                {
                    return HttpNotFound();
                }

                ViewBag.CategoryName = selectedCategory.Name;

                var deals = await _deal.GetDealsByCategoryId(categoryId).ConfigureAwait(false);
                var dealResponses = Mapper.Map<IEnumerable<DealResponse>>(deals);
                return View("DealsList", dealResponses);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Eroare la obținerea anunțurilor după categorie: {ex.Message}");
                if (ex.InnerException != null)
                {
                    System.Diagnostics.Debug.WriteLine($"Detalii: {ex.InnerException.Message}");
                }
                return View("Error");
            }
        }
        
        [AllowAnonymous]
        public async Task<ActionResult> CategoryDetails(int id)
        {
            IEnumerable<DealResponse> dealsForCategory = new List<DealResponse>();
            try
            {
                var categoryModel = await _category.GetCategoryById(id); 

                if (categoryModel == null)
                {
                    return HttpNotFound($"Categoria cu ID-ul {id} nu a fost găsită.");
                }

                ViewBag.CategoryName = categoryModel.Name;

                List<int> subcategoryIds = new List<int>();
                if (categoryModel.Subcategories != null)
                {
                    subcategoryIds = categoryModel.Subcategories.Select(s => s.Id).ToList();
                }

                var dealsFromBL = await _deal.GetDealsBySubcategoryIds(subcategoryIds).ConfigureAwait(false);
                
                if (dealsFromBL != null && dealsFromBL.Any())
                {
                    dealsForCategory = Mapper.Map<IEnumerable<DealResponse>>(dealsFromBL);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Eroare la obținerea anunțurilor pentru categoria {id}: {ex.Message}");
                if (ex.InnerException != null)
                {
                    System.Diagnostics.Debug.WriteLine($"Detalii: {ex.InnerException.Message}");
                }
                dealsForCategory = new List<DealResponse>();
            }

            return View("Category", dealsForCategory);
        }

        [AllowAnonymous]
        public async Task<ActionResult> Search(string query)
        {
            IEnumerable<DealResponse> searchResults = new List<DealResponse>();
            ViewBag.SearchQuery = query;

            try
            {
                if (!string.IsNullOrWhiteSpace(query))
                {
                    var dealsFromBL = await _deal.SearchDeals(query).ConfigureAwait(false);

                    if (dealsFromBL != null && dealsFromBL.Any())
                    {
                        searchResults = Mapper.Map<IEnumerable<DealResponse>>(dealsFromBL);
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Eroare la căutarea anunțurilor pentru '{query}': {ex.Message}");
                if (ex.InnerException != null)
                {
                    System.Diagnostics.Debug.WriteLine($"Detalii: {ex.InnerException.Message}");
                }
                searchResults = new List<DealResponse>();
            }

            return View("Search", searchResults); 
        }

        [AllowAnonymous]
        public ActionResult About()
        {
            return View();
        }

        [AllowAnonymous]
        public ActionResult Privacy()
        {
            return View();
        }

        [AllowAnonymous]
        public ActionResult Contact()
        {
            return View();
        }
    }
}