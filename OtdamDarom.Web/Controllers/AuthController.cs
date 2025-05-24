using System;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using OtdamDarom.BusinessLogic.Dtos;
using OtdamDarom.BusinessLogic.Interfaces;

namespace OtdamDarom.Web.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuth _auth;
        private readonly ISession _session;

        public AuthController()
        {
            var bl = new BusinessLogic.BusinessLogic();
            _auth = bl.GetAuthBL();
            _session = bl.GetSessionBL();
        }

        public ActionResult Login()
        {
            return View();
        }

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> LoginAction(UserLoginRequest request)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { success = false, message = "Invalid data." });
            }

            string dataEmail = request.Email;
            UserAuthResponse response = await _auth.LoginActionAsync(request, dataEmail);

            if (!response.IsSuccess)
            {
                return Json(new { success = false, message = response.StatusMessage });
            }


            var token = await _session.CreateUserSessionAsync(response.Id);

            Session["UserId"] = response.Id;
            Session["UserEmail"] = response.Email;
            Session["Username"] = response.UserName;
            Session["UserRole"] = response.UserRole;
            Session["Token"] = token;

            var cookie = new HttpCookie("AuthToken", token)
            {
                HttpOnly = true,
                Expires = DateTime.Now.AddHours(2)
            };
            Response.Cookies.Add(cookie);


            switch (response.UserRole)
            {
                case "Admin":
                    return RedirectToAction("Dashboard", "Admin");
                case "Artist":
                    return RedirectToAction("Index", "Home");
                default:
                    return RedirectToAction("Index", "Home");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RegisterAction(UserRegisterRequest request)
        {
            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Invalid Data.";
                return Json(new { success = false, message = "Invalid Data." });
            }

            string dataEmail = request.Email;
            var response = await _auth.RegisterActionAsync(request, dataEmail);

            if (!response.IsSuccess)
            {
                TempData["ErrorMessage"] = response.StatusMessage;
                return Json(new { success = false, message = response.StatusMessage });
            }


            var token = await _session.CreateUserSessionAsync(response.Id);

            Session["UserId"] = response.Id;
            Session["UserEmail"] = response.Email;
            Session["Username"] = response.UserName;
            Session["UserRole"] = response.UserRole;
            Session["Token"] = token;

            var cookie = new HttpCookie("AuthToken", token)
            {
                HttpOnly = true,
                Expires = DateTime.Now.AddHours(2)
            };
            Response.Cookies.Add(cookie);

            TempData["SuccessMessage"] = "Account created successfully!";

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
        [ValidateAntiForgeryToken]
        public ActionResult Logout()
        {
            Session.Clear();

            if (Request.Cookies["AuthToken"] != null)
            {
                var cookie = new HttpCookie("AuthToken")
                {
                    Expires = DateTime.Now.AddDays(-1)
                };
                Response.Cookies.Add(cookie);
            }

            return RedirectToAction("Login");

        }
    }
}