using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks; // <<--- ASIGURĂ-TE CĂ ACESTA ESTE PREZENT!
using OtdamDarom.BusinessLogic.Data;
using OtdamDarom.BusinessLogic.Interfaces;
using OtdamDarom.Domain.Models;

namespace OtdamDarom.BusinessLogic.EntityBL
{
    public class CategoryBl : ICategory
    {
        private readonly AppDbContext _context;

        public CategoryBl()
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

        // <<<<<<<<<<<<<<<<< CORECTAREA AICI >>>>>>>>>>>>>>>>>>>>>>
        // Metoda asincronă pentru a prelua o singură categorie după ID
        public async Task<CategoryModel> GetCategoryById(int id) // <<-- ADAUGĂ 'async Task<' AICI
        {
            return await _context.Categories // <<-- ADAUGĂ 'await' AICI
                .Include(c => c.Subcategories)
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == id); // <<-- SCHIMBĂ LA 'FirstOrDefaultAsync'
        }
        // <<<<<<<<<<<<<<<<< SFÂRȘIT CORECTARE >>>>>>>>>>>>>>>>>>>>>>
    }
}