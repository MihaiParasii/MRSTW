using System.Threading.Tasks;
using System.Web.Mvc;
using AutoMapper;
using OtdamDarom.BusinessLogic.Interfaces;
using OtdamDarom.Domain.Models;
using OtdamDarom.Web.Requests;

namespace OtdamDarom.Web.Controllers
{
    public class SubcategoryController : Controller
    {
        private readonly ISubcategory _subcategory;

        public SubcategoryController()
        {
            var bl = new BusinessLogic.BusinessLogic();
            _subcategory = bl.GetSubcategoryBL();
        }
        
        // GET
        public ActionResult Index()
        {
            return View();
        }
        
        public ActionResult AddSubcategory()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddSubcategory(CreateSubcategoryRequest request)
        {
            if (ModelState.IsValid)
            {
                var subcategory = Mapper.Map<SubcategoryModel>(request);

                await _subcategory.CreateSubcategoryAsync(subcategory);

                return RedirectToAction("Index");
            }

            return View(request);
        }


        public async Task<ActionResult> EditSubcategory(int id)
        {
            var subcategory = await _subcategory.GetSubcategoryByIdAsync(id);
            if (subcategory == null)
            {
                return HttpNotFound();
            }

            return View(subcategory);
        }

        [HttpPost]
        public async Task<ActionResult> EditSubcategory(UpdateSubcategoryRequest request)
        {
            if (ModelState.IsValid)
            {
                var subcategoryModel = Mapper.Map<SubcategoryModel>(request);

                await _subcategory.UpdateSubcategoryAsync(subcategoryModel);
                return RedirectToAction("Index");
            }

            return View(request);
        }

        [HttpPost]
        public async Task<JsonResult> DeleteSubcategory(int id)
        {
            var subcategoryModel = await _subcategory.GetSubcategoryByIdAsync(id);
            if (subcategoryModel == null)
            {
                return Json(new { success = false, message = "Product not found." });
            }

            await _subcategory.DeleteSubcategoryAsync(subcategoryModel.Id);

            return Json(new { success = true });
        }
    }
}