// OtdamDarom.Web.Controllers/DealController.cs (sau Home/Controller.cs dacă afișezi acolo)

using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;
using AutoMapper;
using OtdamDarom.BusinessLogic.Interfaces;
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
            var deals = await _deal.GetAll();
            var dealResponses = Mapper.Map<IEnumerable<DealResponse>>(deals);
            return View(dealResponses);
        }
        
        public async Task<ActionResult> Details(int id)
        {
            var dealModel = await _deal.GetById(id);
            if (dealModel == null)
            {
                return HttpNotFound();
            }
            var response = Mapper.Map<DealResponse>(dealModel);
            return View(response);
        }
    }
}