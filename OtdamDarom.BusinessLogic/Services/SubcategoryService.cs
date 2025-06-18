using OtdamDarom.BusinessLogic.Data;
using OtdamDarom.BusinessLogic.Interfaces;

namespace OtdamDarom.BusinessLogic.Services
{
    public class SubcategoryService : ISubcategory
    {
        private readonly AppDbContext _context;

        public SubcategoryService()
        {
            _context = new AppDbContext();
        }
    }
}