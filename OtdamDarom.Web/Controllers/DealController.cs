using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using AutoMapper;
using OtdamDarom.BusinessLogic.Interfaces;
using OtdamDarom.Domain.Models;
using OtdamDarom.Web.Requests;

namespace OtdamDarom.Web.Controllers
{
    public class DealController : Controller
    {
        private IDeal _deal;

        public DealController()
        {
            var bl = new BusinessLogic.BusinessLogic();
            _deal = bl.GetDealBL();
        }

        
        public async Task<ActionResult> Index()
        {
            var dealModels = await _deal.GetAllDealsAsync();

            var responses = dealModels.Select(Mapper.Map<DealResponse>).ToList();

            return View(responses);
        }
        
        public ActionResult AddDeal()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddDeal(CreateDealRequest request)
        {
            if (ModelState.IsValid)
            {
                var dealModel = Mapper.Map<DealModel>(request);
                // TODO: trebuie cumva sa iei ID-ul utilizatorului si sa il pui in deal model aici
                // sau poti sa il pui din BusinessLogic, dar mi se pare ca mai bine e aici.
                // si sa faci fucntie pentru ca adminul sa poata vedea toate Deal-urile, iar fiecare utilizator, doar pe ale lui
                // daca ceva, poti sa te uiti la Nicolae in proiect, sa pizdesti frontend de la ei
                // DE CALITATE SUPERIOARA
                await _deal.CreateDealAsync(dealModel);

                return RedirectToAction("Index");
            }

            return View(request);
        }

        public async Task<ActionResult> EditDeal(int id)
        {
            var dealModel = await _deal.GetDealByIdAsync(id);
            if (dealModel == null)
            {
                return HttpNotFound();
            }

            var response = Mapper.Map<DealResponse>(dealModel);

            return View(response);
        }

        [HttpPost]
        public async Task<ActionResult> EditDeal(UpdateDealRequest request)
        {
            if (ModelState.IsValid)
            {
                var dealModel = Mapper.Map<DealModel>(request);

                await _deal.UpdateDealAsync(dealModel);
                
                return RedirectToAction("Index");
            }

            return View(request);
        }

        [HttpPost]
        public async Task<JsonResult> DeleteDeal(int id)
        {
            var dealModel = await _deal.GetDealByIdAsync(id);
            if (dealModel == null)
            {
                return Json(new { success = false, message = "Product not found." });
            }

            await _deal.DeleteDealAsync(dealModel.Id);

            return Json(new { success = true });
        }
    }
}