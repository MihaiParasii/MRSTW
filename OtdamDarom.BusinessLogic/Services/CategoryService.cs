// OtdamDarom.BusinessLogic.Services/CategoryService.cs
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq; // Păstrăm pentru .Include() și .ToList()
// Eliminăm 'using System.Threading.Tasks;' dacă nu mai sunt metode async

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
                .ToList(); // <<--- Apelăm ToList() (sincron) în loc de ToListAsync()
        }
    }
}