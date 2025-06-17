using System.Collections.Generic;
using System.Threading.Tasks;
using OtdamDarom.Domain.Models;

namespace OtdamDarom.BusinessLogic.Interfaces
{
    public interface ICategory
    {
        // Păstrăm această metodă, e necesară pentru _Sidebar și pentru a obține toate subcategoriile unei categorii
        IEnumerable<CategoryModel> GetAllCategoriesWithSubcategories();

        // <<<<<<<<<<<<<<<<< NOU: Metoda necesară pentru CategoryDetails >>>>>>>>>>>>>>>>>>>>>>
        // Va prelua o singură categorie după ID, inclusiv subcategoriile ei.
        // O facem async Task<CategoryModel> dacă accesul la DB pentru asta e async.
        // Dacă este o operație rapidă în memorie după GetAllCategoriesWithSubcategories(), poate fi sincronă.
        // Presupunând că ai nevoie să o cauți în DB:
        Task<CategoryModel> GetCategoryById(int id); 
        // Dacă GetCategoryById este o operație sincronă (ex: caută în memoria colecției returnate de GetAllCategoriesWithSubcategories),
        // atunci poate fi: CategoryModel GetCategoryById(int id);
    }
}