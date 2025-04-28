using MRSTW.Api.Common;
using MRSTW.Api.Contracts;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Domain.Models.Auth;

namespace MRSTW.Api.Services
{
    public interface IAuthService
    {
        Task RegisterAsync(RegisterRequest request);
        Task<LoginResponse> LoginAsync(LoginRequest request);
    }

    public class AuthService : IAuthService
    {
        // Fake repository până avem baza de date
        private static readonly List<AppUser> _users = new();

        private readonly IConfiguration _configuration;

        public AuthService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task RegisterAsync(RegisterRequest request)
        {
            // Validare parolă și repeat parolă
            if (request.Password != request.RepeatPassword)
            {
                throw new Exception("Passwords do not match.");
            }

            // Verificăm dacă există deja user cu același email
            if (_users.Any(u => u.Email == request.Email))
            {
                throw new Exception("Email already in use.");
            }

            // Creăm hash și salt pentru parolă
            PasswordHasher.CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);

            var user = new AppUser
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                Surname = request.Surname,
                Email = request.Email,
                PhoneNumber = request.PhoneNumber,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt
            };

            _users.Add(user);

            await Task.CompletedTask; // ca să păstrăm async
        }

        public async Task<LoginResponse> LoginAsync(LoginRequest request)
        {
            var user = _users.SingleOrDefault(u => u.Email == request.Email);

            if (user == null)
            {
                throw new Exception("User not found.");
            }

            if (!PasswordHasher.VerifyPasswordHash(request.Password, user.PasswordHash, user.PasswordSalt))
            {
                throw new Exception("Invalid password.");
            }

            var token = GenerateJwtToken(user);

            return await Task.FromResult(new LoginResponse
            {
                Token = token
            });
        }

        private string GenerateJwtToken(AppUser user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, user.Name + " " + user.Surname)
            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Issuer = _configuration["Jwt:Issuer"],
                Audience = _configuration["Jwt:Audience"]
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}
