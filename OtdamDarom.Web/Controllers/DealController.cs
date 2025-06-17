// OtdamDarom.Web.Controllers/DealController.cs

using System; // Asigură-te că ai System pentru Exception
using System.Collections.Generic;
using System.Linq; // Asigură-te că ai System.Linq pentru .Where(), .OrderByDescending(), .Take()
using System.Threading.Tasks;
using System.Web.Mvc;
using AutoMapper;
using OtdamDarom.BusinessLogic.Interfaces;
using OtdamDarom.Web.Requests;
using OtdamDarom.Domain.Models; // Adaugă acest using pentru DealModel

namespace OtdamDarom.Web.Controllers
{
    public class DealController : Controller
    {
        private IDeal _deal;
        // Avem nevoie și de ICategory pentru a accesa detalii despre subcategorii/categorii
        // Dacă GetById din IDeal deja aduce SubcategoryId, atunci ICategory nu e strict necesar aici
        // dar e o bună practică pentru a verifica logic categoriile/subcategoriile.
        // Pentru simplitate, vom folosi doar SubcategoryId direct din DealModel.

        public DealController()
        {
            var bl = new BusinessLogic.BusinessLogic();
            _deal = bl.GetDealBL();
            // _category = bl.GetCategoryBL(); // Nu este necesar aici dacă DealModel are SubcategoryId
        }
        
        public async Task<ActionResult> Index()
        {
            IEnumerable<DealResponse> recentDeals = new List<DealResponse>(); // Inițializăm lista

            try
            {
                var deals = await _deal.GetAll().ConfigureAwait(false);

                if (deals != null && deals.Any())
                {
                    recentDeals = deals
                        .OrderByDescending(d => d.CreationDate)
                        .Take(12) // Păstrăm Take(12) pentru Index, dacă vrei doar cele mai recente
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
                recentDeals = new List<DealResponse>(); // Asigură-te că returnezi o listă goală în caz de eroare
            }

            return View(recentDeals);
        }
        
        // <<<<<<<<<<<<<<<<< MODIFICARE: Acțiunea Details >>>>>>>>>>>>>>>>>>>>>>
        public async Task<ActionResult> Details(int id)
        {
            DealResponse dealResponse = null;
            IEnumerable<DealResponse> similarDeals = new List<DealResponse>();

            try
            {
                // 1. Preluăm detaliile anunțului principal
                var dealModel = await _deal.GetById(id);

                if (dealModel == null)
                {
                    return HttpNotFound($"Anunțul cu ID-ul {id} nu a fost găsit.");
                }

                dealResponse = Mapper.Map<DealResponse>(dealModel);

                // 2. Căutăm anunțuri similare din aceeași subcategorie
                // Verificăm dacă anunțul are o subcategorie și dacă subcategoria are un ID valid
                if (dealModel.SubcategoryId > 0)
                {
                    // Apelăm metoda GetDealsBySubcategoryIds pe care am adăugat-o în IDeal și implementările sale
                    // Pasăm o listă cu un singur ID de subcategorie
                    var dealsFromSameSubcategory = await _deal.GetDealsBySubcategoryIds(new List<int> { dealModel.SubcategoryId });

                    if (dealsFromSameSubcategory != null && dealsFromSameSubcategory.Any())
                    {
                        // Excludem anunțul curent din lista de anunțuri similare
                        similarDeals = dealsFromSameSubcategory
                                        .Where(d => d.Id != dealModel.Id)
                                        .OrderByDescending(d => d.CreationDate) // Ordine descrescătoare după data
                                        .Take(4) // Limitează la 4 anunțuri similare, ca în imaginea 3
                                        .Select(Mapper.Map<DealResponse>)
                                        .ToList();
                    }
                }
                // Dacă nu există subcategorie sau nu s-au găsit suficiente anunțuri similare,
                // lista 'similarDeals' va rămâne goală, iar View-ul va gestiona acest caz.
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Eroare la obținerea detaliilor anunțului sau anunțurilor similare pentru ID {id}: {ex.Message}");
                if (ex.InnerException != null)
                {
                    System.Diagnostics.Debug.WriteLine($"Detalii: {ex.InnerException.Message}");
                }
                // Returnăm un View de eroare sau o listă goală, pentru a nu bloca aplicația
                // Asigură-te că ai un fișier Error.cshtml în Views/Shared sau Views/Deal
                return View("Error");
            }

            // Pasăm anunțurile similare către View prin ViewBag
            ViewBag.SimilarDeals = similarDeals;

            return View(dealResponse);
        }
        // <<<<<<<<<<<<<<<<< SFÂRȘIT MODIFICARE >>>>>>>>>>>>>>>>>>>>>>
    }
}