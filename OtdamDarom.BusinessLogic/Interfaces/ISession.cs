using System.Threading.Tasks;
using OtdamDarom.Domain.Models;

namespace OtdamDarom.BusinessLogic.Interfaces
{
    public interface ISession
    {
        Task<UserSession> CreateUserSession(int userId, bool rememberMe);
        Task<UserSession> GetUserSession(string authToken);
        Task<bool> DeleteUserSession(string authToken);
    }
}