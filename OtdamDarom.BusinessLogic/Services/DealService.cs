using System; // For ArgumentException, InvalidOperationException
using System.Collections.Generic;
using System.Data.Entity; // For Entity Framework methods like ToListAsync, FirstOrDefaultAsync, Include
using System.Linq; // For Where
using System.Threading.Tasks;
using OtdamDarom.BusinessLogic.Data; // For AppDbContext
using OtdamDarom.BusinessLogic.Interfaces; // For IDeal
using OtdamDarom.Domain.Models; // For DealModel

namespace OtdamDarom.BusinessLogic.Services // Or OtdamDarom.BusinessLogic.EntityBL (depending on your folder structure)
{
    public class DealService : IDeal // IMPORTANT: Nu mai extinde UserApi
    {
        private readonly AppDbContext _context; // Instanță privată a contextului bazei de date

        public DealService()
        {
            _context = new AppDbContext(); // Inițializăm contextul bazei de date
        }

        // Implementarea metodelor din IDeal
        public async Task<DealModel> GetById(int id)
        {
            return await _context.Deals.FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<IEnumerable<DealModel>> GetAll()
        {
            return await _context.Deals.AsNoTracking().ToListAsync();
        }

        public async Task Create(DealModel dealData)
        {
            if (dealData == null)
            {
                throw new ArgumentNullException(nameof(dealData), "Deal cannot be null.");
            }
            _context.Deals.Add(dealData);
            await _context.SaveChangesAsync();
        }

        public async Task Update(DealModel dealData)
        {
            if (dealData == null)
            {
                throw new ArgumentNullException(nameof(dealData), "Deal data cannot be null.");
            }

            var existingDeal = await _context.Deals.FirstOrDefaultAsync(u => u.Id == dealData.Id);
            if (existingDeal == null)
            {
                throw new InvalidOperationException($"Deal with ID {dealData.Id} not found for update.");
            }

            // Actualizează proprietățile anunțului existent
            existingDeal.Name = dealData.Name;
            existingDeal.Description = dealData.Description;
            existingDeal.ImageURL = dealData.ImageURL;
            existingDeal.UserId = dealData.UserId;
            existingDeal.SubcategoryId = dealData.SubcategoryId;
            existingDeal.CreationDate = dealData.CreationDate; // Asigură-te că și asta e acolo

            _context.Entry(existingDeal).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task Delete(int dealId)
        {
            var dealToDelete = await _context.Deals.FirstOrDefaultAsync(p => p.Id == dealId);
            if (dealToDelete == null)
            {
                throw new ArgumentException($"Deal with ID {dealId} not found for deletion.");
            }
            _context.Deals.Remove(dealToDelete);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<DealModel>> GetDealsByCategoryId(int categoryId)
        {
            return await _context.Deals
                                 .Include(d => d.Subcategory) // Incluzând Subcategory pentru a accesa CategoryId
                                 .Where(d => d.Subcategory != null && d.Subcategory.CategoryId == categoryId)
                                 .AsNoTracking() // Recomandat pentru operații de citire
                                 .ToListAsync();
        }
    }
}