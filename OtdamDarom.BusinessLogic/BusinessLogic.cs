// OtdamDarom.BusinessLogic/BusinessLogic.cs
using OtdamDarom.BusinessLogic.EntityBL; // Asigură-te că acest using este prezent
using OtdamDarom.BusinessLogic.Interfaces;
using OtdamDarom.BusinessLogic.Services; // Dacă CategoryService e aici, e posibil să nu mai fie folosit pentru categorii

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
            return new CategoryBl(); // <<--- AICI SCHIMBĂ SĂ RETURN EZI NOUA INSTANȚĂ CategoryBl()
        }

        public IDeal GetDealBL()
        {
            return new DealService();
        }

        public ISession GetSessionBL()
        {
            return new SessionService();
        }

        public IUser GetUserBl()
        {
            return new UserService();
        }
    }
}