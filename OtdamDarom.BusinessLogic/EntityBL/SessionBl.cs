using System.Threading.Tasks;
using OtdamDarom.BusinessLogic.Api;
using OtdamDarom.BusinessLogic.Interfaces;
using OtdamDarom.Domain.Models;

namespace OtdamDarom.BusinessLogic.EntityBL
{
    public class SessionBl : ISession
    {
        private readonly UserApi _userApi;

        public SessionBl()
        {
            _userApi = new UserApi();
        }
        
        public async Task<UserSession> CreateUserSession(int userId, bool rememberMe)
        {
            return await _userApi.CreateUserSessionAsync(userId, rememberMe);
        }
        
        public async Task<UserSession> GetUserSession(string authToken)
        {
            return await _userApi.GetActiveUserSessionByTokenAsync(authToken);
        }

        public async Task<bool> DeleteUserSession(string authToken)
        {
            return await _userApi.DeleteUserSessionAsync(authToken);
        }
    }
}