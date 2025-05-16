using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Domain.Models.Auth;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MRSTW.BusinessLogicLayer.Common;
using MRSTW.BusinessLogicLayer.Common.Interfaces;
using MRSTW.BusinessLogicLayer.Contracts;

namespace MRSTW.BusinessLogicLayer.Services;

public interface IAuthService
{
    Task RegisterAsync(RegisterRequest request);
    Task<LoginResponse> LoginAsync(LoginRequest request);
}

public class AuthService(IConfiguration configuration, IBusinessUnitOfWork unitOfWork) : IAuthService
{
    private static readonly List<AppUser> Users = [];

    public async Task RegisterAsync(RegisterRequest request)
    {
        if (request.Password != request.RepeatPassword)
        {
            throw new Exception("Passwords do not match.");
        }

        if (Users.Any(u => u.Email == request.Email))
        {
            throw new Exception("Email already in use.");
        }

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

        await unitOfWork.UserRepository.AddAsync(user);

        await Task.CompletedTask;
    }

    public async Task<LoginResponse> LoginAsync(LoginRequest request)
    {
        var user = await unitOfWork.UserRepository.GetByEmailAsync(request.Email);

        if (user == null)
        {
            throw new Exception("User not found.");
        }

        if (!PasswordHasher.VerifyPasswordHash(request.Password, user.PasswordHash, user.PasswordSalt))
        {
            throw new Exception("Invalid password.");
        }

        string token = GenerateJwtToken(user);

        return await Task.FromResult(new LoginResponse
        {
            Token = token
        });
    }

    private string GenerateJwtToken(AppUser user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(configuration["Jwt:Key"]);

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Email, user.Email),
            new(ClaimTypes.Name, user.Name + " " + user.Surname)
        };

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddHours(1),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
            Issuer = configuration["Jwt:Issuer"],
            Audience = configuration["Jwt:Audience"]
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
    }
}