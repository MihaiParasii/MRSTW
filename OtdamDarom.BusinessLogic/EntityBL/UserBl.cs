using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using OtdamDarom.BusinessLogic.Api;
using OtdamDarom.BusinessLogic.Interfaces;
using OtdamDarom.Domain.Models;
using OtdamDarom.BusinessLogic.Dtos; // Adaugă acest using pentru UserLoginRequest

namespace OtdamDarom.BusinessLogic.EntityBL
{
    public class UserBl : IUser
    {
        private readonly UserApi _userApi;

        public UserBl()
        {
            _userApi = new UserApi();
        }

        public async Task<UserModel> GetUserById(int id)
        {
            return await _userApi.GetUserByIdAsync(id);
        }

        public async Task<UserModel> GetUserByEmail(string email)
        {
            return await _userApi.GetUserByEmailAsync(email);
        }

        public async Task<IEnumerable<UserModel>> GetAllUsers()
        {
            return await _userApi.GetAllUsersAsync();
        }

        public async Task<int> CreateUser(UserModel user)
        {
            return await _userApi.CreateUserAsync(user);
        }

        public async Task UpdateUser(UserModel newUser)
        {
            await _userApi.UpdateUserAsync(newUser);
        }

        public async Task DeleteUser(int id)
        {
            await _userApi.DeleteUserAsync(id);
        }

        public async Task UpdateUserRole(string email, string newRole)
        {
            await _userApi.UpdateUserRoleAsync(email, newRole);
        }

        public async Task<bool> UpdatePassword(int userId, string currentPassword, string newPassword)
        {
            var user = await _userApi.GetUserByIdAsync(userId);
            if (user == null)
            {
                return false;
            }

            var loginRequest = new UserLoginRequest
            {
                Email = user.Email,
                Password = currentPassword
            };

            var authResponse = await _userApi.LoginUserAsync(loginRequest);

            if (!authResponse.IsSuccess)
            {
                return false;
            }

            user.PasswordHash = _userApi.GetComputedHashForPassword(newPassword);

            try
            {
                await _userApi.UpdateUserAsync(user);
                return true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Eroare la schimbarea parolei pentru utilizatorul {userId}: {ex.Message}");
                if (ex.InnerException != null) System.Diagnostics.Debug.WriteLine($"Inner Exception: {ex.InnerException.Message}");
                return false;
            }
        }

        // <<<<<<<<<<<<<<<<< IMPLEMENTARE AICI >>>>>>>>>>>>>>>>>>>>>>
        // Implementarea metodelor pentru categorii și subcategorii, apelând UserApi
        public async Task<IEnumerable<CategoryModel>> GetAllCategoriesWithSubcategories()
        {
            return await _userApi.GetAllCategoriesWithSubcategoriesAsync();
        }

        public async Task<CategoryModel> GetCategoryById(int id)
        {
            return await _userApi.GetCategoryByIdAsync(id);
        }
        // <<<<<<<<<<<<<<<<< SFÂRȘIT IMPLEMENTARE >>>>>>>>>>>>>>>>>>>>>>
    }
}