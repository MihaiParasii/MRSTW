using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using OtdamDarom.BusinessLogic.Data;
using OtdamDarom.BusinessLogic.Dtos;
using OtdamDarom.Domain.Models;

namespace OtdamDarom.BusinessLogic.Api
{
    public class UserApi
    {
        private readonly AppDbContext _context = new AppDbContext();


        public async Task<DealModel> GetDealByIdAsync(int dealId)
        {
            return await _context.Deals.FirstOrDefaultAsync(p => p.Id == dealId);
        }

        public async Task<IEnumerable<DealModel>> GetAllDealsAsync()
        {
            return await _context.Deals.AsNoTracking().ToListAsync();
        }

        public async Task<int> CreateDealAsync(DealModel dealModel)
        {
            if (dealModel == null)
            {
                throw new ArgumentException("Deal cannot be null");
            }

            _context.Deals.Add(dealModel);
            await _context.SaveChangesAsync();

            return dealModel.Id;
        }

        public async Task UpdateDealAsync(DealModel dealModel)
        {
            if (dealModel == null)
            {
                throw new ArgumentException("Deal cannot be found.");
            }

            var existingDeal = await _context.Deals.FirstOrDefaultAsync(u => u.Id == dealModel.Id);

            if (existingDeal == null)
            {
                throw new InvalidOperationException("Deal cannot be found.");
            }

            existingDeal.Name = dealModel.Name;
            existingDeal.Description = dealModel.Description;


            _context.Entry(existingDeal).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteDealAsync(int dealId)
        {
            var deal = await _context.Deals.FirstOrDefaultAsync(p => p.Id == dealId);
            ;
            if (deal == null)
            {
                throw new ArgumentException("Deal not found");
            }

            _context.Deals.Remove(deal);
            await _context.SaveChangesAsync();
        }

        public async Task<string> CreateUserSessionAsync(int userId)
        {
            var token = Guid.NewGuid().ToString();

            var session = new UserSession
            {
                UserId = userId,
                Token = token,
                CreatedAt = DateTime.UtcNow,
                ExpiresAt = DateTime.UtcNow.AddHours(2)
            };

            _context.Sessions.Add(session);
            await _context.SaveChangesAsync();

            return token;
        }

        public async Task UpdateUserAsync(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                throw new ArgumentException("Email cannot be null or empty.", nameof(email));
            }

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);

            if (user != null)
            {
                string token = Guid.NewGuid().ToString();
                
                user.Token = token;

                _context.Entry(user).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
        }

        public async Task<UserAuthResponse> LoginUserAsync(UserLoginRequest request, string dataEmail)
        {
            if (request == null)
            {
                return new UserAuthResponse
                {
                    IsSuccess = false,
                    StatusMessage = "Invalid data."
                };
            }

            try
            {
                if (string.IsNullOrEmpty(request.Email))
                {
                    return new UserAuthResponse
                    {
                        IsSuccess = false,
                        StatusMessage = "Email is required."
                    };
                }

                var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == dataEmail);
                if (user == null)
                {
                    return new UserAuthResponse
                    {
                        IsSuccess = false,
                        StatusMessage = "User does not exists."
                    };
                }

                string hashedPassword = ComputeHash(request.Password);
                if (user.PasswordHash != hashedPassword)
                {
                    return new UserAuthResponse
                    {
                        IsSuccess = false,
                        StatusMessage = "User does not exists or password is incorrect."
                    };
                }

                await UpdateUserAsync(user.Email);


                user = await _context.Users.FirstOrDefaultAsync(u => u.Email == dataEmail);

                return new UserAuthResponse
                {
                    IsSuccess = true,
                    StatusMessage = "Authenticated with success.",
                    Id = user.Id,
                    Email = user.Email,
                    UserName = user.Name,
                    UserRole = user.UserRole,
                    Token = user.Token
                };
            }
            catch (Exception)
            {
                return new UserAuthResponse
                {
                    IsSuccess = false,
                    StatusMessage = "Error."
                };
            }
        }


        public async Task<UserAuthResponse> RegisterUserAsync(UserRegisterRequest request, string email)
        {
            if (request == null)
            {
                return new UserAuthResponse
                {
                    IsSuccess = false,
                    StatusMessage = "Invalid data."
                };
            }

            try
            {
                if (string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.Password) ||
                    string.IsNullOrEmpty(request.Name) || string.IsNullOrEmpty(request.UserRole))
                {
                    return new UserAuthResponse
                    {
                        IsSuccess = false,
                        StatusMessage = "All properties are required."
                    };
                }

                var existingUser = _context.Users.FirstOrDefault(u => u.Email == email);
                if (existingUser != null)
                {
                    return new UserAuthResponse
                    {
                        IsSuccess = false,
                        StatusMessage = "This email is already existing."
                    };
                }

                string hashedPassword = ComputeHash(request.Password);

                var newUser = new UserModel
                {
                    Email = request.Email,
                    Name = request.Name,
                    PasswordHash = hashedPassword,
                    UserRole = request.UserRole,
                    Token = Guid.NewGuid().ToString()
                };

                _context.Users.Add(newUser);
                await _context.SaveChangesAsync();

                return new UserAuthResponse
                {
                    IsSuccess = true,
                    StatusMessage = "Success!",
                    Email = newUser.Email,
                    UserName = newUser.Name,
                    UserRole = newUser.UserRole,
                    Token = newUser.Token
                };
            }
            catch (Exception ex)
            {
                return new UserAuthResponse
                {
                    IsSuccess = false,
                    StatusMessage = $"Error: {ex.Message}"
                };
            }
        }

        private static string ComputeHash(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var bytes = Encoding.UTF8.GetBytes(password);
                var hash = sha256.ComputeHash(bytes);
                return Convert.ToBase64String(hash);
            }
        }
    }
}