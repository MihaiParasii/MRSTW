using OtdamDarom.BusinessLogic.EntityBL;
using OtdamDarom.BusinessLogic.Interfaces;

namespace OtdamDarom.BusinessLogic
{
    public class BusinessLogic
    {

        public IDeal GetDealBL()
        {
            return new DealBl(); 
        }

        public ICategory GetCategoryBL()
        {
            return new CategoryBl(); 
        }

        public IAuth GetAuthBL()
        {
            return new AuthBl(GetSessionBL());
        }

        public ISession GetSessionBL()
        {
            return new SessionBl();
        }

        public IUser GetUserBL()
        {
            return new UserBl(); 
        }
    }
}