// OtdamDarom.BusinessLogic.Interfaces/IAuth.cs
using System.Threading.Tasks;
using OtdamDarom.BusinessLogic.Dtos;
using OtdamDarom.Domain.Models; 

namespace OtdamDarom.BusinessLogic.Interfaces
{
    public interface IAuth
    {
        Task<UserAuthResponse> Register(UserRegisterRequest request);
        Task<UserAuthResponse> Login(UserLoginRequest request);
        Task<bool> Logout(string authToken);
        Task<UserModel> GetCurrentUser(string authToken); 
        // NOU: Metoda pentru schimbarea parolei
    }
}