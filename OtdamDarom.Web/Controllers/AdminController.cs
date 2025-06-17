using System.Threading.Tasks;
using System.Web.Mvc;
using OtdamDarom.Domain.Models; // Pentru UserModel, dacă este folosit direct
using OtdamDarom.BusinessLogic.Interfaces; // Asigură-te că ai această directivă using

namespace OtdamDarom.Web.Controllers
{
    [Authorize(Roles = "Admin")] // Asigură-te că doar administratorii pot accesa acest controler
    public class AdminController : Controller
    {
        private readonly BusinessLogic.BusinessLogic _businessLogic;
        private readonly IUser _userBl; // Utilizăm interfața IUser

        public AdminController()
        {
            _businessLogic = new BusinessLogic.BusinessLogic();
            _userBl = _businessLogic.GetUserBL(); // ATENȚIE AICI: Numele metodei este GetUserBL() (cu BL mare)
        }

        // GET: Admin/Dashboard
        public ActionResult Dashboard()
        {
            return View();
        }

        // GET: Admin/Users
        public async Task<ActionResult> Users()
        {
            // Apelează GetAllUsers() fără Async la sfârșit
            var users = await _userBl.GetAllUsers(); 
            return View(users);
        }

        // GET: Admin/UserDetails/5
        public async Task<ActionResult> UserDetails(int id)
        {
            // Apelează GetUserById() fără Async la sfârșit
            var user = await _userBl.GetUserById(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // GET: Admin/CreateUser
        public ActionResult CreateUser()
        {
            return View(new UserModel()); // Sau un DTO specific pentru creare
        }

        // POST: Admin/CreateUser
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateUser(UserModel user) // Sau un DTO
        {
            if (ModelState.IsValid)
            {
                // Apelează CreateUser() fără Async la sfârșit
                await _userBl.CreateUser(user); 
                TempData["SuccessMessage"] = "Utilizatorul a fost creat cu succes!";
                return RedirectToAction("Users");
            }
            TempData["ErrorMessage"] = "Eroare la crearea utilizatorului. Verificați datele introduse.";
            return View(user);
        }

        // GET: Admin/EditUser/5
        public async Task<ActionResult> EditUser(int id)
        {
            // Apelează GetUserById() fără Async la sfârșit
            var user = await _userBl.GetUserById(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: Admin/EditUser
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditUser(UserModel newUser) // Sau un DTO
        {
            if (ModelState.IsValid)
            {
                // Apelează UpdateUser() fără Async la sfârșit
                await _userBl.UpdateUser(newUser); 
                TempData["SuccessMessage"] = "Utilizatorul a fost actualizat cu succes!";
                return RedirectToAction("Users");
            }
            TempData["ErrorMessage"] = "Eroare la actualizarea utilizatorului. Verificați datele introduse.";
            return View(newUser);
        }

        // POST: Admin/DeleteUser/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteUser(int id)
        {
            // Apelează DeleteUser() fără Async la sfârșit
            await _userBl.DeleteUser(id); 
            TempData["SuccessMessage"] = "Utilizatorul a fost șters cu succes!";
            return RedirectToAction("Users");
        }

        // GET: Admin/EditUserRole/email
        public async Task<ActionResult> EditUserRole(string email)
        {
            // Apelează GetAllUsers() fără Async la sfârșit pentru a popula dropdown-ul
            ViewBag.Users = new SelectList(await _userBl.GetAllUsers(), "Email", "Email"); // Afișează email ca text și valoare
            return View();
        }

        // POST: Admin/UpdateUserRole
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> UpdateUserRole(string email, string newRole)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(newRole))
            {
                TempData["ErrorMessage"] = "Emailul și rolul nou sunt obligatorii.";
                // Apelează GetAllUsers() fără Async la sfârșit pentru a repopula dropdown-ul în caz de eroare
                ViewBag.Users = new SelectList(await _userBl.GetAllUsers(), "Email", "Email");
                return View("EditUserRole");
            }

            // Apelează UpdateUserRole() fără Async la sfârșit
            await _userBl.UpdateUserRole(email, newRole);
            TempData["SuccessMessage"] = $"Rolul utilizatorului {email} a fost actualizat la {newRole} cu succes!";
            return RedirectToAction("Users");
        }

        // POST: Admin/DeleteSelectedUsers (Exemplu pentru ștergere multiplă)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteSelectedUsers(int[] selectedUserIds)
        {
            if (selectedUserIds != null && selectedUserIds.Length > 0)
            {
                foreach (var id in selectedUserIds)
                {
                    // Apelează DeleteUser() fără Async la sfârșit
                    await _userBl.DeleteUser(id);
                }
                TempData["SuccessMessage"] = $"{selectedUserIds.Length} utilizatori au fost șterși cu succes!";
            }
            else
            {
                TempData["ErrorMessage"] = "Niciun utilizator nu a fost selectat pentru ștergere.";
            }
            return RedirectToAction("Users");
        }
    }
}