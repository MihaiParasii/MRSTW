using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

using OtdamDarom.BusinessLogic.Data;
using OtdamDarom.BusinessLogic.Interfaces;
using OtdamDarom.Domain.Models;

namespace OtdamDarom.BusinessLogic.Services
{
    public class CategoryService : ICategory
    {
        private readonly AppDbContext _context;

        public CategoryService()
        {
            _context = new AppDbContext();
        }
        
        public IEnumerable<CategoryModel> GetAllCategoriesWithSubcategories()
        {
            return _context.Categories
                .Include(c => c.Subcategories)
                .AsNoTracking()
                .ToList();
        }
        public async Task<CategoryModel> GetCategoryById(int id)
        {
            return await _context.Categories
                .Include(c => c.Subcategories)
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == id);
        }
    }
}