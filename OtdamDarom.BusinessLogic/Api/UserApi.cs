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
        
        internal async Task<DealModel> GetByIdAsync(int id)
        {
            return await _context.Deals.FirstOrDefaultAsync(p => p.Id == id);
        }

        internal async Task<IEnumerable<DealModel>> GetAllAsync()
        {
            return await _context.Deals.AsNoTracking().ToListAsync();
        }

        internal async Task CreateDealAsync(DealModel dealModel)
        {
            if (dealModel == null) throw new ArgumentException("Deal cannot be null");
            _context.Deals.Add(dealModel);
            await _context.SaveChangesAsync();
        }

        internal async Task UpdateDealAsync(DealModel dealModel)
        {
            if (dealModel == null) throw new ArgumentException("Deal cannot be found.");
            var existingDeal = await _context.Deals.FirstOrDefaultAsync(u => u.Id == dealModel.Id);
            if (existingDeal == null) throw new InvalidOperationException("Deal cannot be found.");

            existingDeal.Name = dealModel.Name;
            existingDeal.Description = dealModel.Description;
            existingDeal.ImageURL = dealModel.ImageURL;
            existingDeal.UserId = dealModel.UserId;
            existingDeal.SubcategoryId = dealModel.SubcategoryId; 
            existingDeal.CreationDate = dealModel.CreationDate;

            _context.Entry(existingDeal).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        internal async Task DeleteDealAsync(int dealId)
        {
            var deal = await _context.Deals.FirstOrDefaultAsync(p => p.Id == dealId);
            if (deal == null) throw new ArgumentException("Deal not found");
            _context.Deals.Remove(deal);
            await _context.SaveChangesAsync();
        }

        internal async Task<IEnumerable<DealModel>> GetDealsByCategoryIdAsync(int categoryId)
        {
            return await _context.Deals
                                 .Include(d => d.Subcategory)
                                 .Where(d => d.Subcategory != null && d.Subcategory.CategoryId == categoryId)
                                 .AsNoTracking()
                                 .ToListAsync();
        }

        internal async Task<IEnumerable<DealModel>> GetDealsBySubcategoryIdsAsync(List<int> subcategoryIds)
        {
            if (subcategoryIds == null || !subcategoryIds.Any())
            {
                return new List<DealModel>();
            }

            return await _context.Deals
                                 .Where(d => d.SubcategoryId.HasValue && subcategoryIds.Contains(d.SubcategoryId.Value))
                                 .AsNoTracking()
                                 .ToListAsync();
        }

        internal async Task<IEnumerable<DealModel>> GetDealsByUserIdAsync(int userId)
        {
            return await _context.Deals
                                 .Where(d => d.UserId == userId)
                                 .OrderByDescending(d => d.CreationDate)
                                 .AsNoTracking()
                                 .ToListAsync();
        }
        internal async Task<IEnumerable<CategoryModel>> GetAllCategoriesWithSubcategoriesAsync()
        {
            return await _context.Categories
                                 .Include(c => c.Subcategories)
                                 .AsNoTracking()
                                 .ToListAsync();
        }

        internal async Task<CategoryModel> GetCategoryByIdAsync(int id)
        {
            return await _context.Categories
                                 .Include(c => c.Subcategories)
                                 .AsNoTracking()
                                 .FirstOrDefaultAsync(c => c.Id == id);
        }
        
        internal async Task<UserAuthResponse> RegisterUserAsync(UserRegisterRequest request)
        {
            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email);
            if (existingUser != null)
            {
                return new UserAuthResponse { IsSuccess = false, StatusMessage = "Un utilizator cu această adresă de email există deja." };
            }

            var passwordHash = GetComputedHashForPassword(request.Password);

            var newUser = new UserModel
            {
                Email = request.Email,
                Name = request.Name,
                PasswordHash = passwordHash,
                UserRole = request.UserRole,
                CreationDate = DateTime.UtcNow,
                ProfilePictureUrl = "/Content/Images/default-user.png"
            };

            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();

            return new UserAuthResponse
            {
                IsSuccess = true,
                StatusMessage = "Înregistrare reușită.",
                Id = newUser.Id,
                Email = newUser.Email,
                UserName = newUser.Name,
                UserRole = newUser.UserRole,
                ProfilePictureUrl = newUser.ProfilePictureUrl
            };
        }
        
        internal async Task<UserAuthResponse> LoginUserAsync(UserLoginRequest request)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email);

            if (user == null)
            {
                return new UserAuthResponse { IsSuccess = false, StatusMessage = "Email sau parolă incorectă." };
            }

            var hashedInputPassword = GetComputedHashForPassword(request.Password);
            if (user.PasswordHash != hashedInputPassword)
            {
                return new UserAuthResponse { IsSuccess = false, StatusMessage = "Email sau parolă incorectă." };
            }

            return new UserAuthResponse
            {
                IsSuccess = true,
                StatusMessage = "Autentificare reușită.",
                Id = user.Id,
                Email = user.Email,
                UserName = user.Name,
                UserRole = user.UserRole,
                ProfilePictureUrl = user.ProfilePictureUrl
            };
        }
        
        internal async Task<UserSession> CreateUserSessionAsync(int userId, bool rememberMe)
        {
            var existingSessions = await _context.Sessions.Where(s => s.UserId == userId).ToListAsync();
            _context.Sessions.RemoveRange(existingSessions);
            await _context.SaveChangesAsync();

            var token = Guid.NewGuid().ToString();
            var session = new UserSession
            {
                UserId = userId,
                Token = token,
                CreatedAt = DateTime.UtcNow,
                ExpiresAt = DateTime.UtcNow.Add(rememberMe ? TimeSpan.FromDays(30) : TimeSpan.FromHours(2))
            };
            _context.Sessions.Add(session);
            await _context.SaveChangesAsync();
            return session;
        }
        
        internal async Task<UserSession> GetActiveUserSessionByTokenAsync(string token)
        {
            if (string.IsNullOrEmpty(token)) return null;

            return await _context.Sessions
                                 .Include(s => s.User)
                                 .FirstOrDefaultAsync(s => s.Token == token && s.ExpiresAt > DateTime.UtcNow);
        }
        
        internal async Task<bool> DeleteUserSessionAsync(string token)
        {
            var session = await _context.Sessions.FirstOrDefaultAsync(s => s.Token == token);
            if (session != null)
            {
                _context.Sessions.Remove(session);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }
        
        internal string GetComputedHashForPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var bytes = Encoding.UTF8.GetBytes(password);
                var hash = sha256.ComputeHash(bytes);
                return Convert.ToBase64String(hash);
            }
        }

        internal async Task<IEnumerable<DealModel>> SearchDealsAsync(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                return new List<DealModel>();
            }
            string lowerQuery = query.ToLower();
            return await _context.Deals
                                 .Where(d => d.Name.ToLower().Contains(lowerQuery) || 
                                             d.Description.ToLower().Contains(lowerQuery))
                                 .AsNoTracking()
                                 .ToListAsync();
        }
        
        internal async Task<UserModel> GetUserByIdAsync(int id)
        {
            return await _context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Id == id);
        }

        internal async Task<UserModel> GetUserByEmailAsync(string email)
        {
            return await _context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Email == email);
        }

        internal async Task<IEnumerable<UserModel>> GetAllUsersAsync()
        {
            return await _context.Users.AsNoTracking().ToListAsync();
        }

        internal async Task<int> CreateUserAsync(UserModel user)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));
            
            if (!string.IsNullOrEmpty(user.PasswordHash) && user.PasswordHash.Length < 60)
            {
                user.PasswordHash = GetComputedHashForPassword(user.PasswordHash);
            }
            user.CreationDate = DateTime.UtcNow;
            
            if (string.IsNullOrEmpty(user.ProfilePictureUrl))
            {
                 user.ProfilePictureUrl = "/Content/Images/default-user.png";
            }

            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user.Id;
        }

        internal async Task UpdateUserAsync(UserModel newUser)
        {
            if (newUser == null) throw new ArgumentNullException(nameof(newUser));
            
            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Id == newUser.Id);
            if (existingUser == null) throw new InvalidOperationException("User not found.");

            existingUser.Name = newUser.Name;
            existingUser.Email = newUser.Email;
            existingUser.UserRole = newUser.UserRole;
            existingUser.ProfilePictureUrl = newUser.ProfilePictureUrl; 
            
            if (!string.IsNullOrEmpty(newUser.PasswordHash) && newUser.PasswordHash != existingUser.PasswordHash)
            {
                existingUser.PasswordHash = newUser.PasswordHash; 
            }

            _context.Entry(existingUser).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        internal async Task DeleteUserAsync(int id)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
            if (user == null) throw new ArgumentException("User not found.", nameof(id));
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
        }

        internal async Task UpdateUserRoleAsync(string email, string newRole)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null) throw new ArgumentException("User not found.", nameof(email));
            user.UserRole = newRole;
            _context.Entry(user).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
    }
}