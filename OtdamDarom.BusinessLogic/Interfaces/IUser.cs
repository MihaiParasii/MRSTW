using System.Collections.Generic;
using System.Threading.Tasks;
using OtdamDarom.BusinessLogic.Dtos;
using OtdamDarom.Domain.Models;

namespace OtdamDarom.BusinessLogic.Interfaces
{
    public interface IUser
    {
        Task<UserModel> GetUserById(int id);
        Task<UserModel> GetUserByEmail(string email);
        Task<IEnumerable<UserModel>> GetAllUsers();
        Task<int> CreateUser(UserModel user);
        Task UpdateUser(UserModel newUser);
        Task DeleteUser(int id);
        Task UpdateUserRole(string email, string newRole);
        Task<bool> UpdatePassword(int userId, string currentPassword, string newPassword);

        // <<<<<<<<<<<<<<<<< ADAUGATE AICI >>>>>>>>>>>>>>>>>>>>>>
        Task<IEnumerable<CategoryModel>> GetAllCategoriesWithSubcategories();
        Task<CategoryModel> GetCategoryById(int id);
        // <<<<<<<<<<<<<<<<< SFÂRȘIT ADAUGATE >>>>>>>>>>>>>>>>>>>>>>
    }
}