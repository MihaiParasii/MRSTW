using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Security.Principal;
using System.Threading.Tasks;
using OtdamDarom.BusinessLogic.Interfaces;
using OtdamDarom.Domain.Models;
using System.Linq;
using System.Security.Claims;

namespace OtdamDarom.Web.Filters
{
    public class CustomAuthorizeAttribute : AuthorizeAttribute
    {
        private IAuth _auth;

        public CustomAuthorizeAttribute()
        {
            var bl = new BusinessLogic.BusinessLogic();
            _auth = bl.GetAuthBL();
        }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            var request = httpContext.Request;
            var authTokenCookie = request.Cookies["AuthToken"];

            if (authTokenCookie == null || string.IsNullOrEmpty(authTokenCookie.Value))
            {
                return false; 
            }

            var authToken = authTokenCookie.Value;
            UserModel currentUser = null;

            try
            {
                currentUser = Task.Run(() => _auth.GetCurrentUser(authToken)).Result;
            }
            catch (AggregateException ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error getting current user in CustomAuthorize (AggregateException): {ex.InnerException?.Message ?? ex.Message}");
                currentUser = null; 
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"General error getting current user in CustomAuthorize: {ex.Message}");
                currentUser = null; 
            }
            
            if (currentUser == null)
            {
                if (authTokenCookie != null)
                {
                    authTokenCookie.Expires = DateTime.Now.AddDays(-1);
                    authTokenCookie.HttpOnly = true;
                    authTokenCookie.Secure = request.IsSecureConnection;
                    authTokenCookie.SameSite = SameSiteMode.Lax;
                    authTokenCookie.Path = "/";
                    httpContext.Response.Cookies.Add(authTokenCookie);
                }
                return false; 
            }

            var identity = new ClaimsIdentity(
                new GenericIdentity(currentUser.Email, "CustomTokenAuth").Claims,
                "CustomTokenAuth"
            );
            identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, currentUser.Id.ToString()));
            identity.AddClaim(new Claim(ClaimTypes.Name, currentUser.Name));
            identity.AddClaim(new Claim(ClaimTypes.Email, currentUser.Email));
            identity.AddClaim(new Claim(ClaimTypes.Role, currentUser.UserRole)); 
            identity.AddClaim(new Claim("UserProfilePicUrl", currentUser.ProfilePictureUrl ?? "")); 

            var principal = new GenericPrincipal(identity, new string[] { currentUser.UserRole });
            httpContext.User = principal; 

            if (!string.IsNullOrEmpty(Roles))
            {
                var requiredRoles = Roles.Split(',').Select(r => r.Trim()).ToList();
                return requiredRoles.Any(role => httpContext.User.IsInRole(role));
            }

            return true; 
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            if (filterContext.IsChildAction)
            {
                filterContext.Result = new EmptyResult(); 
                return; 
            }

            var actionDescriptor = filterContext.ActionDescriptor as ReflectedActionDescriptor;
            if (actionDescriptor != null && actionDescriptor.MethodInfo.GetCustomAttributes(typeof(AllowAnonymousAttribute), true).Any())
            {
                return; 
            }

            if (filterContext.HttpContext.Request.IsAjaxRequest())
            {
                filterContext.Result = new HttpStatusCodeResult(401); 
            }
            else
            {
                filterContext.Result = new RedirectToRouteResult(
                    new RouteValueDictionary(
                        new { controller = "Auth", action = "Login", returnUrl = filterContext.HttpContext.Request.Url.PathAndQuery }
                    )
                );
            }
        }
    }
}