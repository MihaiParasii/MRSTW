using System.Threading.Tasks;

namespace OtdamDarom.BusinessLogic.Interfaces
{
    public interface ISession
    {
        Task<string> CreateUserSessionAsync(int userId);
    }
}