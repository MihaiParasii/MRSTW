using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using OtdamDarom.BusinessLogic.Data;
using OtdamDarom.Domain.Models;

namespace OtdamDarom.BusinessLogic.Api
{
    public class AdminApi
    {
        private readonly AppDbContext _context = new AppDbContext();

        public async Task<UserModel> GetUserByIdAsync(int id)
        {
            var model = await _context.Users.AsNoTracking().FirstOrDefaultAsync(p => p.Id == id);

            return model;
        }

        internal async Task<IEnumerable<UserModel>> GetAllUsersAsync()
        {
            return await _context.Users.AsNoTracking().ToListAsync();
        }

        internal async Task  UpdateUserAsync(UserModel user)
        {
            if (user == null)
            {
                throw new ArgumentException("User cannot be null.");
            }

            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Id == user.Id);
            if (existingUser == null)
            {
                throw new ArgumentException("User not found");
            }

            existingUser.Name = user.Name;
            existingUser.Email = user.Email;
            existingUser.UserRole = user.UserRole;


            _context.Entry(existingUser).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        internal async Task<int> CreateUserAsync(UserModel userData)
        {
            if (userData == null)
            {
                throw new ArgumentException("User cannot be null");
            }


            _context.Users.Add(userData);
            await _context.SaveChangesAsync();

            return userData.Id;
        }

        internal async Task DeleteUserAsync(int userid)
        {
            var entity = await _context.Users.FirstOrDefaultAsync(p => p.Id == userid);
            ;
            if (entity == null)
            {
                throw new ArgumentException("User not found");
            }

            _context.Users.Remove(entity);
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
            ;
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

        public async Task<SubcategoryModel> GetSubcategoryByIdAsync(int id)
        {
            var subcategory = await _context.Subcategories.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);

            return subcategory;
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

            _context.Entry(existingSubcategory).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteSubcategoryAsync(int subcategoryId)
        {
            var subcategory = await _context.Subcategories.FirstOrDefaultAsync(p => p.Id == subcategoryId);
            ;
            if (subcategory == null)
            {
                throw new ArgumentException("Subcategory not found");
            }

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
    }
}