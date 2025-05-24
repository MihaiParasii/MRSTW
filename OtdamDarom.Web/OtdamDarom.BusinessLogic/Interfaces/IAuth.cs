using System.Threading.Tasks;
using OtdamDarom.BusinessLogic.Dtos;

namespace OtdamDarom.BusinessLogic.Interfaces
{
    public interface IAuth
    {
        Task<UserAuthResponse> LoginActionAsync(UserLoginRequest request, string email);
        Task<UserAuthResponse> RegisterActionAsync(UserRegisterRequest request, string email);
    }
}