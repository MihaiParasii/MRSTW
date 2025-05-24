using OtdamDarom.BusinessLogic.Interfaces;
using OtdamDarom.BusinessLogic.Services;

namespace OtdamDarom.BusinessLogic
{
    public class BusinessLogic
    {
        public IAuth GetAuthBL()
        {
            return new AuthService();
        }

        public ICategory GetCategoryBL()
        {
            return new CategoryService();
        }

        public IDeal GetDealBL()
        {
            return new DealService();
        }

        public ISession GetSessionBL()
        {
            return new SessionService();
        }

        public ISubcategory GetSubcategoryBL()
        {
            return new SubcategoryService();
        }

        public IUser GetUserBl()
        {
            return new UserService();
        }
    }
}