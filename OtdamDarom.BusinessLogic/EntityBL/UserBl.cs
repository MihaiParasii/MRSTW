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

        // <<<<<<<<<<<<<<<<<<<<<<<< AU FOST ELIMINATE DE AICI >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
        // Metoda de hashing simplificată (HashPassword)
        // Metoda de verificare a parolei (VerifyPassword)
        // Acestea se află acum EXCLUSIV în UserApi.cs sub numele GetComputedHashForPassword.
        // <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<< >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>

        public async Task<UserModel> GetUserById(int id)
        {
            return await _userApi.GetUserByIdAsync(id);
        }

        public async Task<IEnumerable<UserModel>> GetAllUsers()
        {
            return await _userApi.GetAllUsersAsync();
        }

        public async Task<int> CreateUser(UserModel user)
        {
            // IMPORTANT: UserApi.CreateUserAsync se ocupă de hashing-ul parolei.
            // Asigură-te că 'user.PasswordHash' conține parola în text clar când ajunge aici.
            // UserApi.CreateUserAsync va verifica dacă e deja hashedă sau o va hasha.
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

        // NOU: Implementarea metodei de actualizare a parolei
        public async Task<bool> UpdatePassword(int userId, string currentPassword, string newPassword)
        {
            // Pasul 1: Preluăm informațiile despre utilizator.
            var user = await _userApi.GetUserByIdAsync(userId);
            if (user == null)
            {
                return false; // Utilizatorul nu a fost găsit.
            }

            // Pasul 2: Verificăm parola actuală folosind logica de login din UserApi.
            // Aceasta va asigura că folosim același algoritm de hashing (Base64)
            // și aceeași logică de verificare ca la login.
            var loginRequest = new UserLoginRequest
            {
                Email = user.Email,       // Folosim email-ul utilizatorului găsit
                Password = currentPassword // Parola actuală în text clar, așa cum e introdusă de utilizator
            };

            var authResponse = await _userApi.LoginUserAsync(loginRequest);

            if (!authResponse.IsSuccess)
            {
                // Parola actuală este incorectă (verificarea a eșuat în UserApi).
                return false;
            }

            // Pasul 3: Dacă parola actuală este corectă, hash-ezi noua parolă
            // folosind metoda de hashing din UserApi și o salvezi.
            user.PasswordHash = _userApi.GetComputedHashForPassword(newPassword);

            try
            {
                await _userApi.UpdateUserAsync(user); // Salvează modificările parolei în baza de date.
                return true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Eroare la schimbarea parolei pentru utilizatorul {userId}: {ex.Message}");
                // Poți loga excepția mai detaliat sau o poți arunca mai departe, dacă este necesar.
                return false;
            }
        }
    }
}