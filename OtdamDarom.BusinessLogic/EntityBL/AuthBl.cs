using System.Threading.Tasks;
using OtdamDarom.BusinessLogic.Api;
using OtdamDarom.BusinessLogic.Dtos;
using OtdamDarom.BusinessLogic.Interfaces;
using OtdamDarom.Domain.Models;

namespace OtdamDarom.BusinessLogic.EntityBL
{
    public class AuthBl : IAuth
    {
        private readonly UserApi _userApi; // Instanțiem direct UserApi pentru a accesa metodele interne
        private readonly ISession _sessionBl; // Injectăm ISession pentru a gestiona sesiunile

        public AuthBl(ISession sessionBl)
        {
            _userApi = new UserApi(); // Inițializează UserApi
            _sessionBl = sessionBl; // Primește ISession prin constructor
        }

        public async Task<UserAuthResponse> Register(UserRegisterRequest request)
        {
            var response = await _userApi.RegisterUserAsync(request);

            if (response.IsSuccess)
            {
                // Creează sesiunea și adaugă tokenul la răspuns
                var session = await _sessionBl.CreateUserSession(response.Id, false); // La înregistrare, inițial nu RememberMe
                response.AuthToken = session.Token;
            }
            return response;
        }

        public async Task<UserAuthResponse> Login(UserLoginRequest request)
        {
            var response = await _userApi.LoginUserAsync(request);

            if (response.IsSuccess)
            {
                // Creează sesiunea și adaugă tokenul la răspuns
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
            return session?.User; // Returnează userul din sesiune
        }
    }
}