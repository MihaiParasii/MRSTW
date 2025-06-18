using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web; // Adăugat pentru HttpPostedFileBase
using OtdamDarom.BusinessLogic.Data;
using OtdamDarom.BusinessLogic.Dtos; // DTOs-urile pot fi utile aici pentru mapări
using OtdamDarom.Domain.Models;

namespace OtdamDarom.BusinessLogic.Api
{
    public class AdminApi
    {
        private readonly AppDbContext _context = new AppDbContext();

        public async Task<int> GetTotalUsersCountAsync()
        {
            return await _context.Users.CountAsync();
        }

        public async Task<int> GetTotalDealsCountAsync()
        {
            return await _context.Deals.CountAsync();
        }

        public async Task<IEnumerable<UserModel>> GetLatestUsersAsync(int count = 5)
        {
            return await _context.Users
                                 .OrderByDescending(u => u.Id)
                                 .Take(count)
                                 .AsNoTracking()
                                 .ToListAsync();
        }

        public async Task<IEnumerable<DealModel>> GetLatestDealsAsync(int count = 5)
        {
            return await _context.Deals
                                 .Include(d => d.User)
                                 .OrderByDescending(d => d.Id)
                                 .Take(count)
                                 .AsNoTracking()
                                 .ToListAsync();
        }

        public async Task<UserModel> GetUserByIdAsync(int id)
        {
            var model = await _context.Users.AsNoTracking().FirstOrDefaultAsync(p => p.Id == id);
            return model;
        }

        public async Task<IEnumerable<UserModel>> GetAllUsersAsync()
        {
            return await _context.Users.AsNoTracking().ToListAsync();
        }

        public async Task UpdateUserAsync(UserModel userModel, HttpPostedFileBase imageFile, bool deleteExistingImage)
        {
            if (userModel == null)
            {
                throw new ArgumentException("User model cannot be null.");
            }

            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Id == userModel.Id);
            if (existingUser == null)
            {
                throw new ArgumentException("Utilizatorul nu a fost găsit.");
            }

            // Actualizează doar proprietățile permise pentru editare
            existingUser.Name = userModel.Name;
            existingUser.Email = userModel.Email;
            // existingUser.UserRole = userModel.UserRole; // Dacă rolul se editează separat, scoate linia aceasta

            // Logica de gestionare a imaginii de profil
            if (deleteExistingImage)
            {
                if (!string.IsNullOrEmpty(existingUser.ProfilePictureUrl) && !existingUser.ProfilePictureUrl.Contains("default-user.png"))
                {
                    var oldPath = System.Web.HttpContext.Current.Server.MapPath(existingUser.ProfilePictureUrl);
                    if (System.IO.File.Exists(oldPath))
                    {
                        System.IO.File.Delete(oldPath);
                    }
                }
                existingUser.ProfilePictureUrl = null; // Setăm la null sau la imaginea implicită
            }
            else if (imageFile != null && imageFile.ContentLength > 0)
            {
                if (!string.IsNullOrEmpty(existingUser.ProfilePictureUrl) && !existingUser.ProfilePictureUrl.Contains("default-user.png"))
                {
                    var oldPath = System.Web.HttpContext.Current.Server.MapPath(existingUser.ProfilePictureUrl);
                    if (System.IO.File.Exists(oldPath))
                    {
                        System.IO.File.Delete(oldPath);
                    }
                }

                var fileName = Guid.NewGuid().ToString() + System.IO.Path.GetExtension(imageFile.FileName);
                var uploadPath = System.Web.HttpContext.Current.Server.MapPath("~/Content/Images/Uploads");
                if (!System.IO.Directory.Exists(uploadPath))
                {
                    System.IO.Directory.CreateDirectory(uploadPath);
                }
                var path = System.IO.Path.Combine(uploadPath, fileName);
                imageFile.SaveAs(path);
                existingUser.ProfilePictureUrl = "~/Content/Images/Uploads/" + fileName;
            }

            _context.Entry(existingUser).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task<int> CreateUserAsync(UserModel userData)
        {
            if (userData == null)
            {
                throw new ArgumentException("User cannot be null");
            }

            // Asigură-te că parola este hashed dacă este furnizată
            if (!string.IsNullOrEmpty(userData.PasswordHash) && userData.PasswordHash.Length < 60) // Verificare simplă pentru a nu re-hash-ui un hash existent
            {
                userData.PasswordHash = GetComputedHashForPassword(userData.PasswordHash);
            }
            else if (string.IsNullOrEmpty(userData.PasswordHash))
            {
                // Poate ar trebui să arunci o eroare sau să setezi o parolă implicită dacă e obligatorie
                throw new ArgumentException("Parola este obligatorie pentru un utilizator nou.");
            }

            userData.CreationDate = DateTime.UtcNow;

            if (string.IsNullOrEmpty(userData.ProfilePictureUrl))
            {
                userData.ProfilePictureUrl = "/Content/Images/default-user.png";
            }

            _context.Users.Add(userData);
            await _context.SaveChangesAsync();

            return userData.Id;
        }

        public async Task DeleteUserAsync(int userId)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null)
            {
                throw new ArgumentException("Utilizatorul nu a fost găsit.");
            }

            // Șterge anunțurile utilizatorului și imaginile asociate
            var userDeals = await _context.Deals.Where(d => d.UserId == userId).ToListAsync();
            if (userDeals.Any())
            {
                foreach (var deal in userDeals)
                {
                    if (!string.IsNullOrEmpty(deal.ImageURL) && !deal.ImageURL.Contains("default-deal.png"))
                    {
                        var imagePath = System.Web.HttpContext.Current.Server.MapPath(deal.ImageURL);
                        if (System.IO.File.Exists(imagePath))
                        {
                            System.IO.File.Delete(imagePath);
                        }
                    }
                }
                _context.Deals.RemoveRange(userDeals);
            }

            // Șterge imaginea de profil a utilizatorului
            if (!string.IsNullOrEmpty(user.ProfilePictureUrl) && !user.ProfilePictureUrl.Contains("default-user.png"))
            {
                var profilePicPath = System.Web.HttpContext.Current.Server.MapPath(user.ProfilePictureUrl);
                if (System.IO.File.Exists(profilePicPath))
                {
                    System.IO.File.Delete(profilePicPath);
                }
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteDealAsync(int dealId)
        {
            var deal = await _context.Deals.FirstOrDefaultAsync(d => d.Id == dealId);
            if (deal == null)
            {
                throw new ArgumentException("Anunțul nu a fost găsit.");
            }

            // Șterge imaginea anunțului
            if (!string.IsNullOrEmpty(deal.ImageURL) && !deal.ImageURL.Contains("default-deal.png"))
            {
                var imagePath = System.Web.HttpContext.Current.Server.MapPath(deal.ImageURL);
                if (System.IO.File.Exists(imagePath))
                {
                    System.IO.File.Delete(imagePath);
                }
            }

            _context.Deals.Remove(deal);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateUserRoleAsync(string email, string newRole)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                throw new ArgumentException("Email cannot be null.");
            }

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null)
            {
                throw new ArgumentException("User not found.");
            }

            user.UserRole = newRole;

            _context.Entry(user).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<CategoryModel>> GetAllCategoriesAsync()
        {
            var categories = await _context.Categories.AsNoTracking().ToListAsync();
            return categories;
        }

        public async Task<CategoryModel> GetCategoryByIdAsync(int id)
        {
            var category = await _context.Categories.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
            return category;
        }

        public async Task UpdateCategoryAsync(CategoryModel category)
        {
            if (category == null)
            {
                throw new ArgumentException("Category cannot be null.");
            }

            var existingCategory = await _context.Categories.FirstOrDefaultAsync(u => u.Id == category.Id);
            if (existingCategory == null)
            {
                throw new ArgumentException("Category not found");
            }

            existingCategory.Name = category.Name;

            _context.Entry(existingCategory).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteCategoryAsync(int categoryId)
        {
            var category = await _context.Categories.FirstOrDefaultAsync(p => p.Id == categoryId);
            if (category == null)
            {
                throw new ArgumentException("Category not found");
            }

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
        }

        public async Task<int> CreateCategoryAsync(CategoryModel category)
        {
            if (category == null)
            {
                throw new ArgumentException("Category cannot be null");
            }

            _context.Categories.Add(category);
            await _context.SaveChangesAsync();

            return category.Id;
        }

        public async Task<IEnumerable<SubcategoryModel>> GetAllSubcategoriesAsync()
        {
            var subcategories = await _context.Subcategories.AsNoTracking().ToListAsync();
            return subcategories;
        }

        // METODA CORECTATĂ: GetDealByIdAsync - Include Subcategory și Category pentru a accesa CategoryId
        public async Task<DealModel> GetDealByIdAsync(int id)
        {
            return await _context.Deals
                                 .Include(d => d.Subcategory.Category) // Include Category prin Subcategory
                                 .AsNoTracking()
                                 .FirstOrDefaultAsync(d => d.Id == id);
        }

        // Metoda actualizată pentru UpdateDealAsync pentru a lucra cu DealModel (chiar dacă vine din DTO în Controller)
        // Logica de actualizare a imaginii și a câmpurilor text este aici
        public async Task UpdateDealAsync(DealModel dealModel, HttpPostedFileBase imageFile, bool deleteExistingImage)
        {
            if (dealModel == null)
            {
                throw new ArgumentException("Deal model cannot be null.");
            }

            var existingDeal = await _context.Deals.FirstOrDefaultAsync(d => d.Id == dealModel.Id);
            if (existingDeal == null)
            {
                throw new ArgumentException("Anunțul nu a fost găsit.");
            }

            existingDeal.Name = dealModel.Name;
            existingDeal.Description = dealModel.Description;
            existingDeal.SubcategoryId = dealModel.SubcategoryId; // Folosește SubcategoryId primit din model

            // Logica de gestionare a imaginii anunțului
            if (deleteExistingImage)
            {
                if (!string.IsNullOrEmpty(existingDeal.ImageURL) && !existingDeal.ImageURL.Contains("default-deal.png"))
                {
                    var oldPath = System.Web.HttpContext.Current.Server.MapPath(existingDeal.ImageURL);
                    if (System.IO.File.Exists(oldPath))
                    {
                        System.IO.File.Delete(oldPath);
                    }
                }
                existingDeal.ImageURL = null; // Setăm la null sau la imaginea implicită
            }
            else if (imageFile != null && imageFile.ContentLength > 0)
            {
                if (!string.IsNullOrEmpty(existingDeal.ImageURL) && !existingDeal.ImageURL.Contains("default-deal.png"))
                {
                    var oldPath = System.Web.HttpContext.Current.Server.MapPath(existingDeal.ImageURL);
                    if (System.IO.File.Exists(oldPath))
                    {
                        System.IO.File.Delete(oldPath);
                    }
                }

                var fileName = Guid.NewGuid().ToString() + System.IO.Path.GetExtension(imageFile.FileName);
                var uploadPath = System.Web.HttpContext.Current.Server.MapPath("~/Content/Images/Uploads");
                if (!System.IO.Directory.Exists(uploadPath))
                {
                    System.IO.Directory.CreateDirectory(uploadPath);
                }
                var path = System.IO.Path.Combine(uploadPath, fileName);
                imageFile.SaveAs(path);
                existingDeal.ImageURL = "~/Content/Images/Uploads/" + fileName;
            }

            _context.Entry(existingDeal).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task<SubcategoryModel> GetSubcategoryByIdAsync(int id)
        {
            var subcategory = await _context.Subcategories.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
            return subcategory;
        }

        public async Task<IEnumerable<SubcategoryModel>> GetSubcategoriesByCategoryIdAsync(int categoryId)
        {
            return await _context.Subcategories.Where(s => s.CategoryId == categoryId).AsNoTracking().ToListAsync();
        }

        public async Task UpdateSubcategoryAsync(SubcategoryModel subcategory)
        {
            if (subcategory == null)
            {
                throw new ArgumentException("Subcategory cannot be null.");
            }

            var existingSubcategory = await _context.Subcategories.FirstOrDefaultAsync(u => u.Id == subcategory.Id);
            if (existingSubcategory == null)
            {
                throw new ArgumentException("Subcategory not found");
            }

            existingSubcategory.Name = subcategory.Name;
            existingSubcategory.CategoryId = subcategory.CategoryId; // Asigură-te că CategoryId este actualizat

            _context.Entry(existingSubcategory).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteSubcategoryAsync(int subcategoryId)
        {
            var subcategory = await _context.Subcategories.FirstOrDefaultAsync(p => p.Id == subcategoryId);
            if (subcategory == null)
            {
                throw new ArgumentException("Subcategory not found");
            }

            // TODO: Adaugă logică pentru a gestiona anunțurile asociate înainte de ștergere
            // De exemplu, poți seta SubcategoryId la null pentru anunțurile aferente sau le poți șterge.
            // Altfel, vei primi o eroare de cheie străină.

            _context.Subcategories.Remove(subcategory);
            await _context.SaveChangesAsync();
        }

        public async Task<int> CreateSubcategoryAsync(SubcategoryModel subcategory)
        {
            if (subcategory == null)
            {
                throw new ArgumentException("Subcategory cannot be null");
            }

            _context.Subcategories.Add(subcategory);
            await _context.SaveChangesAsync();

            return subcategory.Id;
        }

        private string GetComputedHashForPassword(string password)
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