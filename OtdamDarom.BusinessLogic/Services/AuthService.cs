using System.Threading.Tasks;
using OtdamDarom.BusinessLogic.Api;
using OtdamDarom.BusinessLogic.Dtos;
using OtdamDarom.BusinessLogic.Interfaces;

namespace OtdamDarom.BusinessLogic.Services
{
    public class AuthService : UserApi, IAuth
    {
        public async Task<UserAuthResponse> LoginActionAsync(UserLoginRequest request, string email)
        {
            return await LoginUserAsync(request, email);
        }

        public async  Task<UserAuthResponse> RegisterActionAsync(UserRegisterRequest request, string email)
        {
            return await RegisterUserAsync(request, email);
        }
    }
}