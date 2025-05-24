using System.Collections.Generic;
using System.Threading.Tasks;
using OtdamDarom.Domain.Models;

namespace OtdamDarom.BusinessLogic.Interfaces
{
    public interface IUser
    {
        Task<UserModel> GetUserByIdAsync(int id);
        Task<IEnumerable<UserModel>> GetAllUsersAsync();
        Task<int> CreateUserAsync(UserModel user);
        Task UpdateUserAsync(UserModel newUser);
        Task DeleteUserAsync(int id);
        Task UpdateUserRoleAsync(string email, string newRole);
    }
}