using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;
using OtdamDarom.BusinessLogic.Data;
using OtdamDarom.BusinessLogic.Interfaces;
using OtdamDarom.Domain.Models;

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