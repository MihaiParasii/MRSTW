using System.Threading.Tasks;
using OtdamDarom.BusinessLogic.Api;
using OtdamDarom.BusinessLogic.Dtos;
using OtdamDarom.BusinessLogic.Interfaces;
using OtdamDarom.Domain.Models; // Pentru UserModel

namespace OtdamDarom.BusinessLogic.EntityBL
{
    public class AuthBl : IAuth
    {
        private readonly UserApi _userApi;
        private readonly ISession _sessionBl;

        public AuthBl(ISession sessionBl)
        {
            _userApi = new UserApi();
            _sessionBl = sessionBl;
        }

        public async Task<UserAuthResponse> Register(UserRegisterRequest request)
        {
            // UserApi.RegisterUserAsync va gestiona hashing-ul parolei
            var response = await _userApi.RegisterUserAsync(request);

            if (response.IsSuccess)
            {
                var session = await _sessionBl.CreateUserSession(response.Id, false);
                response.AuthToken = session.Token;
            }
            return response;
        }

        public async Task<UserAuthResponse> Login(UserLoginRequest request)
        {
            // UserApi.LoginUserAsync va gestiona verificarea parolei.
            // Acum se bazează pe aceeași metodă de hashing ca UpdatePassword.
            var response = await _userApi.LoginUserAsync(request);

            if (response.IsSuccess)
            {
                var session = await _sessionBl.CreateUserSession(response.Id, request.RememberMe);
                response.AuthToken = session.Token;
            }
            return response;
        }

        public async Task<bool> Logout(string authToken)
        {
            return await _sessionBl.DeleteUserSession(authToken);
        }

        public async Task<UserModel> GetCurrentUser(string authToken)
        {
            var session = await _sessionBl.GetUserSession(authToken);
            return session?.User;
        }
    }
}