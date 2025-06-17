using System.Threading.Tasks;
using OtdamDarom.Domain.Models; // Pentru UserSession

namespace OtdamDarom.BusinessLogic.Interfaces
{
    public interface ISession
    {
        Task<UserSession> CreateUserSession(int userId, bool rememberMe); // Returnează UserSession, nu doar tokenul
        Task<UserSession> GetUserSession(string authToken);
        Task<bool> DeleteUserSession(string authToken);
    }
}