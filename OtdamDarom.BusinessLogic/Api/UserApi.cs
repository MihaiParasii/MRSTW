// OtdamDarom.BusinessLogic.Api/UserApi.cs
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

        // --- Metode pentru Deals (Rămân așa cum sunt, dar sunt apelate de DealBl) ---
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
        // -------------------------------------------------

        // NOU: Metoda pentru a obține Deals pe CategoryId (accesibilă prin DealBl)
        internal async Task<IEnumerable<DealModel>> GetDealsByCategoryIdAsync(int categoryId)
        {
            return await _context.Deals
                                 .Include(d => d.Subcategory) // Trebuie să includem Subcategory pentru a accesa CategoryId
                                 .Where(d => d.Subcategory != null && d.Subcategory.CategoryId == categoryId)
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
            if (string.IsNullOrWhiteSpace(email)) throw new ArgumentException("Email cannot be null or empty.", nameof(email));
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
            return null;
        }

        public async Task<UserAuthResponse> RegisterUserAsync(UserRegisterRequest request, string email)
        {
            return null;
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