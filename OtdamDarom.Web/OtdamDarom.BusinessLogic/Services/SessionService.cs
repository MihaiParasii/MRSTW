using System.Threading.Tasks;
using OtdamDarom.BusinessLogic.Api;
using OtdamDarom.BusinessLogic.Interfaces;

namespace OtdamDarom.BusinessLogic.Services
{
    public class SessionService : UserApi, ISession
    {
        public new async Task<string> CreateUserSessionAsync(int userId)
        {
            return await base.CreateUserSessionAsync(userId);
        }
    }
}