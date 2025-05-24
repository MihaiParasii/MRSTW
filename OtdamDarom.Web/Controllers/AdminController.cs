using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using OtdamDarom.BusinessLogic.Interfaces;
using OtdamDarom.BusinessLogic.Services;
using OtdamDarom.Domain.Models;
using OtdamDarom.Web.Requests;

namespace OtdamDarom.Web.Controllers
{
    public class AdminController : Controller
    {
        private readonly IUser _user;

        public AdminController()
        {
            var bl = new BusinessLogic.BusinessLogic();
            _user = bl.GetUserBl();
        }

        public async Task<ActionResult> Index()
        {
            var users = await _user.GetAllUsersAsync();
            var userDtos = users.Select(Mapper.Map<UserResponse>).ToList();
            
            return View(userDtos);
        }

        public async Task<ActionResult> UserDetails(int id)
        {
            var user = await _user.GetUserByIdAsync(id);
            if (user == null)
            {
                return HttpNotFound();
            }

            var dto = Mapper.Map<UserResponse>(user);
            return View(dto);
        }

        public ActionResult CreateUser()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateUser(CreateUserRequest request)
        {
            if (!ModelState.IsValid)
            {
                return View(request);
            }

            var user = Mapper.Map<UserModel>(request);
            await _user.CreateUserAsync(user);
            return RedirectToAction("Index");
        }

        public async Task<ActionResult> EditUser(int id)
        {
            var user = await _user.GetUserByIdAsync(id);
            if (user == null)
            {
                return HttpNotFound();
            }

            var dto = Mapper.Map<UpdateUserRequest>(user);
            return View(dto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditUser(int id, UpdateUserRequest request)
        {
            if (!ModelState.IsValid)
            {
                return View(request);
            }

            var existing = await _user.GetUserByIdAsync(id);
            if (existing == null)
            {
                return HttpNotFound();
            }

            var updated = Mapper.Map<UserModel>(request);
            updated.Id = id;

            await _user.UpdateUserAsync(updated);
            return RedirectToAction("Index");
        }

        [HttpPost, ActionName("DeleteUser")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            await _user.DeleteUserAsync(id);
            return RedirectToAction("Index");
        }

        public async Task<ActionResult> ChangeRole(string email, string newRole)
        {
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(newRole))
            {
                return new HttpStatusCodeResult(400, "Invalid input.");
            }

            await _user.UpdateUserRoleAsync(email, newRole);
            return RedirectToAction("Index");
        }

        public ActionResult Dashboard()
        {
            return View();
        }

        public async Task<ActionResult> ManageUsers()
        {
            var users = await _user.GetAllUsersAsync();
            return View(users);
        }

        public ActionResult AddUser()
        {
            return View();
        }


        [HttpPost]
        public async Task<ActionResult> DeleteUser(int id)
        {
            await _user.DeleteUserAsync(id);
            return RedirectToAction("ManageUsers");
        }

        [HttpPost]
        public async Task<ActionResult> UpdateUserRole(int id, string role)
        {
            var user = await _user.GetUserByIdAsync(id);
            if (user != null)
            {
                user.UserRole = role;
                await _user.UpdateUserRoleAsync(user.Email, role);
                return Json(new { success = true });
            }

            return Json(new { success = false });
        }
    }
}