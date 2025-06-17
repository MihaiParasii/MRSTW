using System.Threading.Tasks;
using OtdamDarom.BusinessLogic.Api;
using OtdamDarom.BusinessLogic.Interfaces;
using OtdamDarom.Domain.Models; // Pentru UserSession

namespace OtdamDarom.BusinessLogic.EntityBL
{
    // NU MAI EXTINDE UserApi. Folosește-l prin compoziție (has-a).
    public class SessionBl : ISession
    {
        private readonly UserApi _userApi; // Adăugăm o instanță a UserApi

        public SessionBl()
        {
            _userApi = new UserApi(); // Inițializăm UserApi în constructor
        }

        // Metoda CreateUserSession primește acum și parametrul 'rememberMe'
        public async Task<UserSession> CreateUserSession(int userId, bool rememberMe)
        {
            return await _userApi.CreateUserSessionAsync(userId, rememberMe);
        }

        // Adaugă metodele GetUserSession și DeleteUserSession definite în ISession
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