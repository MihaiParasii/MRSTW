using System.Collections.Generic;
using System.Threading.Tasks;
using OtdamDarom.Domain.Models; 

namespace OtdamDarom.BusinessLogic.Interfaces
{
    public interface IUser
    {
        Task<UserModel> GetUserById(int id);
        Task<IEnumerable<UserModel>> GetAllUsers();
        Task<int> CreateUser(UserModel user);
        Task UpdateUser(UserModel newUser); 
        Task DeleteUser(int id);
        Task UpdateUserRole(string email, string newRole);
        // NOU: Metoda pentru actualizarea parolei
        Task<bool> UpdatePassword(int userId, string currentPassword, string newPassword);
    }
}