using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks; // Păstrăm acest using pentru alte metode async (Index, DealsByCategory)
using System.Web.Mvc;
using AutoMapper;
using OtdamDarom.BusinessLogic.Interfaces;
using OtdamDarom.Web.Requests;
using OtdamDarom.Domain.Models;

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

        // Acțiunea Index poate rămâne asincronă, nu este apelată de RenderAction
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

        // <<--- ACȚIUNEA _Sidebar ESTE ACUM SINCROANĂ
        [ChildActionOnly] 
        public ActionResult _Sidebar() // <<-- Fără 'async' și 'Task<>'
        {
            IEnumerable<CategoryResponse> categoryResponses = new List<CategoryResponse>();
            try
            {
                // Apelăm metoda sincronă din CategoryService, fără 'await'
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

        // Acțiunea DealsByCategory poate rămâne asincronă, dar va apela metoda sincronă a categoriilor
        public async Task<ActionResult> DealsByCategory(int categoryId)
        {
            try
            {
                // Apelăm metoda sincronă, deci fără 'await' aici
                var allCategoriesWithSubcategories = _category.GetAllCategoriesWithSubcategories();
                var selectedCategory = allCategoriesWithSubcategories.FirstOrDefault(c => c.Id == categoryId);
                
                if (selectedCategory == null)
                {
                    return HttpNotFound(); 
                }

                ViewBag.CategoryName = selectedCategory.Name;

                // Aceasta este încă asincronă, presupunând că _deal.GetDealsByCategoryId() este async
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
    }
}