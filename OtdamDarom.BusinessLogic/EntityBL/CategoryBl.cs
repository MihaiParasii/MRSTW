using System.Collections.Generic;
using System.Data.Entity;
using System.Linq; // Necesită acest using pentru Include și ToList
using OtdamDarom.BusinessLogic.Data; // Necesită AppDbContext
using OtdamDarom.BusinessLogic.Interfaces; // Pentru ICategory
using OtdamDarom.Domain.Models;

namespace OtdamDarom.BusinessLogic.EntityBL
{
    public class CategoryBl : ICategory // Acum CategoryBl implementează interfața ICategory sincronă
    {
        private readonly AppDbContext _context; // Injectăm (sau instanțiem) AppDbContext direct

        public CategoryBl()
        {
            _context = new AppDbContext(); // Instanțiem contextul bazei de date
        }
        
        // Metoda este acum complet sincronă, conform ICategory
        public IEnumerable<CategoryModel> GetAllCategoriesWithSubcategories()
        {
            return _context.Categories
                .Include(c => c.Subcategories) // Include subcategoriile
                .AsNoTracking() // Recomandat pentru operații de citire
                .ToList(); // Apelăm metoda sincronă ToList()
        }
    }
}