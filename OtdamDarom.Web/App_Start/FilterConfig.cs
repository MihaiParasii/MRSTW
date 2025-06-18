using System.Web.Mvc;
using OtdamDarom.Web.Filters;

namespace OtdamDarom.Web
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new CustomAuthorizeAttribute()); 
        }
    }
}