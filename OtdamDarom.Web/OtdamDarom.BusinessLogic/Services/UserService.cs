using System.Collections.Generic;
using System.Threading.Tasks;
using OtdamDarom.BusinessLogic.Api;
using OtdamDarom.BusinessLogic.Interfaces;
using OtdamDarom.Domain.Models;

namespace OtdamDarom.BusinessLogic.Services
{
    public class UserService : AdminApi, IUser
    {
        public new async Task<UserModel> GetUserByIdAsync(int id)
        {
            return await base.GetUserByIdAsync(id);
        }

        public new async Task<IEnumerable<UserModel>> GetAllUsersAsync()
        {
            return await base.GetAllUsersAsync();
        }

        public new async Task<int> CreateUserAsync(UserModel user)
        {
            return await base.CreateUserAsync(user);
        }

        public new async Task UpdateUserAsync(UserModel newUser)
        {
            await base.UpdateUserAsync(newUser);
        }

        public new async Task DeleteUserAsync(int id)
        {
            await base.DeleteUserAsync(id);
        }

        public new async Task UpdateUserRoleAsync(string email, string newRole)
        {
            await base.UpdateUserRoleAsync(email, newRole);
        }
    }
}