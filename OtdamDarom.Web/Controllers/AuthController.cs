using System;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using OtdamDarom.BusinessLogic.Dtos; 
using OtdamDarom.BusinessLogic.Interfaces;
using OtdamDarom.Domain.Models; 

namespace OtdamDarom.Web.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuth _auth;
        private readonly ISession _session;
        private readonly IUser _user;

        public AuthController()
        {
            var bl = new BusinessLogic.BusinessLogic();
            _auth = bl.GetAuthBL();
            _session = bl.GetSessionBL();
            _user = bl.GetUserBL();
        }

        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<ActionResult> LoginAction(UserLoginRequest request, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.ReturnUrl = returnUrl;
                return View("Login", request);
            }

            var response = await _auth.Login(request); 

            if (!response.IsSuccess)
            {
                ModelState.AddModelError("", response.StatusMessage);
                ViewBag.ReturnUrl = returnUrl;
                return View("Login", request);
            }

            SetAuthCookie(response.AuthToken, request.RememberMe);

            Session["UserId"] = response.Id;
            Session["UserEmail"] = response.Email;
            Session["Username"] = response.UserName;
            Session["UserRole"] = response.UserRole;
            Session["UserProfilePicUrl"] = response.ProfilePictureUrl; 
            
            TempData["SuccessMessage"] = "Autentificare reușită!";

            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }

            switch (response.UserRole)
            {
                case "Admin":
                    return RedirectToAction("Dashboard", "Admin");
                case "Artist": 
                    return RedirectToAction("Index", "Home");
                case "User":
                    return RedirectToAction("Index", "Home");
                default:
                    return RedirectToAction("Index", "Home");
            }
        }

        [AllowAnonymous]
        public ActionResult Register()
        {
            return View(new UserRegisterRequest { UserRole = "User" });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<ActionResult> RegisterAction(UserRegisterRequest request)
        {
            if (!ModelState.IsValid)
            {
                return View("Register", request);
            }

            request.UserRole = "User"; 

            var response = await _auth.Register(request);

            if (!response.IsSuccess)
            {
                ModelState.AddModelError("", response.StatusMessage);
                return View("Register", request);
            }

            SetAuthCookie(response.AuthToken, false); 

            Session["UserId"] = response.Id;
            Session["UserEmail"] = response.Email;
            Session["Username"] = response.UserName;
            Session["UserRole"] = response.UserRole;
            Session["UserProfilePicUrl"] = response.ProfilePictureUrl;

            TempData["SuccessMessage"] = "Contul a fost creat cu succes! Te-ai autentificat.";

            switch (response.UserRole)
            {
                case "Admin":
                    return RedirectToAction("Dashboard", "Admin");
                case "User":
                    return RedirectToAction("Index", "Home");
                default:
                    return RedirectToAction("Index", "Home");
            }
        }
        
        [HttpPost]
        public async Task<ActionResult> Logout()
        {
            var authToken = Request.Cookies["AuthToken"]?.Value;
            if (!string.IsNullOrEmpty(authToken))
            {
                await _auth.Logout(authToken);
            }
            
            if (Request.Cookies["AuthToken"] != null)
            {
                var cookie = new HttpCookie("AuthToken")
                {
                    Expires = DateTime.Now.AddDays(-1),
                    HttpOnly = true, 
                    Secure = Request.IsSecureConnection, 
                    SameSite = SameSiteMode.Lax 
                };
                Response.Cookies.Add(cookie);
            }

            Session.Clear();
            Session.Abandon();

            TempData["SuccessMessage"] = "Ai fost deconectat cu succes.";
            return RedirectToAction("Index", "Home"); 
        }

        private void SetAuthCookie(string token, bool rememberMe)
        {
            var cookie = new HttpCookie("AuthToken", token)
            {
                HttpOnly = true,
                Secure = Request.IsSecureConnection,
                SameSite = SameSiteMode.Lax 
            };

            if (rememberMe)
            {
                cookie.Expires = DateTime.Now.AddDays(30);
            }
            else
            {
                cookie.Expires = DateTime.Now.AddHours(2);
            }

            Response.Cookies.Add(cookie);
        }

        [ChildActionOnly]
        public async Task<ActionResult> GetLoggedInUser()
        {
            var authToken = Request.Cookies["AuthToken"]?.Value;
            UserModel currentUser = null;

            if (!string.IsNullOrEmpty(authToken))
            {
                currentUser = await _auth.GetCurrentUser(authToken);
                if (currentUser != null)
                {
                    Session["UserId"] = currentUser.Id;
                    Session["UserEmail"] = currentUser.Email;
                    Session["Username"] = currentUser.Name;
                    Session["UserRole"] = currentUser.UserRole;
                    Session["UserProfilePicUrl"] = currentUser.ProfilePictureUrl; 
                } else {
                    Response.Cookies["AuthToken"].Expires = DateTime.Now.AddDays(-1);
                    var expiredCookie = new HttpCookie("AuthToken") 
                    { 
                        Expires = DateTime.Now.AddDays(-1), 
                        HttpOnly = true, 
                        Secure = Request.IsSecureConnection, 
                        SameSite = SameSiteMode.Lax,
                        Path = "/" 
                    };
                    Response.Cookies.Add(expiredCookie);

                    Session.Clear();
                    Session.Abandon();
                }
            }
            return PartialView("_LoggedInUserPartial", currentUser);
        }
    }
}