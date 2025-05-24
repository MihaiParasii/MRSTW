using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using AutoMapper;
using OtdamDarom.BusinessLogic.Interfaces;
using OtdamDarom.Domain.Models;
using OtdamDarom.Web.Requests;

namespace OtdamDarom.Web.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ICategory _category;

        public CategoryController()
        {
            var bl = new BusinessLogic.BusinessLogic();
            _category = bl.GetCategoryBL();
        }

        public async Task<ActionResult> Index()
        {
            var categories = await _category.GetAllCategoriesAsync();
            var dto = categories.Select(Mapper.Map<CategoryResponse>).ToList();
            return View(dto);
        }

        public async Task<ActionResult> Details(int id)
        {
            var category = await _category.GetCategoryByIdAsync(id);
            if (category == null)
            {
                return HttpNotFound();
            }

            var dto = Mapper.Map<CategoryResponse>(category);
            return View(dto);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(CreateCategoryRequest request)
        {
            var category = Mapper.Map<CategoryModel>(request);
            await _category.CreateCategoryAsync(category);
            return RedirectToAction("Index");
        }

        public async Task<ActionResult> Edit(int id)
        {
            var category = await _category.GetCategoryByIdAsync(id);
            if (category == null)
            {
                return HttpNotFound();
            }

            var dto = new UpdateCategoryRequest { Name = category.Name };
            return View(dto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, UpdateCategoryRequest request)
        {
            if (!ModelState.IsValid) return View(request);

            var existing = await _category.GetCategoryByIdAsync(id);
            if (existing == null)
            {
                return HttpNotFound();
            }

            existing.Name = request.Name;
            await _category.UpdateCategoryAsync(existing);
            return RedirectToAction("Index");
        }

        public async Task<ActionResult> Delete(int id)
        {
            var category = await _category.GetCategoryByIdAsync(id);
            if (category == null)
            {
                return HttpNotFound();
            }

            var dto = Mapper.Map<CategoryResponse>(category);
            return View(dto);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            await _category.DeleteCategoryAsync(id);
            return RedirectToAction("Index");
        }
    }
}