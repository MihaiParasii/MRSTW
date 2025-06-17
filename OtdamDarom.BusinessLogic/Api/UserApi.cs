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
                                 .Where(d => subcategoryIds.Contains(d.SubcategoryId))
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

        // <<<<<<<<<<<<<<<<< METODE PENTRU AUTENTIFICARE ȘI SESIUNI - REFACTORIZATE >>>>>>>>>>>>>>>>>>>>>>

        // Înregistrează un utilizator nou
        internal async Task<UserAuthResponse> RegisterUserAsync(UserRegisterRequest request)
        {
            // Verifică dacă email-ul există deja
            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email);
            if (existingUser != null)
            {
                return new UserAuthResponse { IsSuccess = false, StatusMessage = "Un utilizator cu această adresă de email există deja." };
            }

            // Hashing parola (folosind metoda GetComputedHashForPassword din această clasă)
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

        // Loghează un utilizator existent
        internal async Task<UserAuthResponse> LoginUserAsync(UserLoginRequest request)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email);

            if (user == null)
            {
                return new UserAuthResponse { IsSuccess = false, StatusMessage = "Email sau parolă incorectă." };
            }

            // Verifică parola (folosind metoda GetComputedHashForPassword din această clasă)
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

        // Creează o sesiune pentru un utilizator
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

        // Găsește o sesiune activă după token
        internal async Task<UserSession> GetActiveUserSessionByTokenAsync(string token)
        {
            if (string.IsNullOrEmpty(token)) return null;

            return await _context.Sessions
                                 .Include(s => s.User)
                                 .FirstOrDefaultAsync(s => s.Token == token && s.ExpiresAt > DateTime.UtcNow);
        }

        // Delogare - șterge sesiunea
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

        // <<<<<<<<<<<<<<<<<<<<<<<< METODA DE HASHING UNIFICATĂ >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
        // Acum este internal pentru a putea fi apelată din UserBl.
        // Asigură-te că ESTE ACEEAȘI METODĂ FOLOSITĂ PENTRU ÎNREGISTRARE ȘI LOGIN.
        internal string GetComputedHashForPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var bytes = Encoding.UTF8.GetBytes(password);
                var hash = sha256.ComputeHash(bytes);
                return Convert.ToBase64String(hash); // PĂSTRĂM BASE64 PENTRU CONSISTENȚĂ
            }
        }
        // <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<< >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>

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

        // NOU: Metode pentru UserService (din AdminApi)
        internal async Task<UserModel> GetUserByIdAsync(int id)
        {
            return await _context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Id == id);
        }

        internal async Task<IEnumerable<UserModel>> GetAllUsersAsync()
        {
            return await _context.Users.AsNoTracking().ToListAsync();
        }

        internal async Task<int> CreateUserAsync(UserModel user)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));
            
            // Asigură-te că parola este hashedă DOAR dacă nu este deja (ex: la crearea manuală a unui user)
            // UserApi.RegisterUserAsync deja hashează, deci acest bloc e un "fallback"
            // pentru alte metode de creare a utilizatorilor care ar putea apela direct CreateUserAsync.
            if (!string.IsNullOrEmpty(user.PasswordHash) && user.PasswordHash.Length < 60) // Estimăm lungimea unui hash Base64 (ar fi ~44 caractere)
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
            
            // ATENȚIE: Nu modifica parola hash direct aici, decât dacă newUser.PasswordHash
            // conține o parolă NOUĂ hashedă care trebuie salvată (caz rar, mai degrabă folosești UpdatePassword)
            if (!string.IsNullOrEmpty(newUser.PasswordHash) && newUser.PasswordHash != existingUser.PasswordHash)
            {
                // Aici ar trebui să ajungă doar hash-ul NOULUI parole (dacă e cazul)
                // sau e un caz de resetare parolă, unde deja ai hash-ul
                // Dacă vine parola în clar aici, trebuie hashedă:
                // existingUser.PasswordHash = GetComputedHashForPassword(newUser.PasswordHash);
                // Dar cel mai bine e să te bazezi pe UpdatePassword din UserBl pentru asta.
                existingUser.PasswordHash = newUser.PasswordHash; // Păstrăm hash-ul nou, dacă a fost modificat de UpdatePassword
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