using System.Collections.Generic;
using System.Data.Entity; // Necesare pentru .Include()
using System.Linq;      // Necesare pentru .FirstOrDefault()
using System.Threading.Tasks; // <<--- Foarte important: Acesta este necesar pentru metodele async

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

        // Metoda este acum complet sincronă
        public IEnumerable<CategoryModel> GetAllCategoriesWithSubcategories()
        {
            return _context.Categories
                .Include(c => c.Subcategories)
                .AsNoTracking() // AsNoTracking este o bună practică pentru citire
                .ToList(); // Apelăm ToList() (sincron) în loc de ToListAsync()
        }

        // <<<<<<<<<<<<<<<<< NOU: Implementarea GetCategoryById >>>>>>>>>>>>>>>>>>>>>>
        // Am făcut-o asincronă, așa cum este definită în ICategory.
        public async Task<CategoryModel> GetCategoryById(int id)
        {
            // Interogăm baza de date pentru o singură categorie, inclusiv subcategoriile ei.
            // Folosim FirstOrDefaultAsync pentru a obține un singur element sau null.
            return await _context.Categories
                .Include(c => c.Subcategories) // Include subcategoriile pentru a le avea disponibile
                .AsNoTracking() // Fără tracking pentru performanță, deoarece e o operație de citire
                .FirstOrDefaultAsync(c => c.Id == id);
        }
        // <<<<<<<<<<<<<<<<< SFÂRȘIT NOU >>>>>>>>>>>>>>>>>>>>>>
    }
}