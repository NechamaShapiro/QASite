using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using QASite.Data;
using QASite.Web.Models;
using System.Security.Claims;

namespace QASite.Web.Controllers
{
    public class AccountController : Controller
    {
        private string _connectionString;

        public AccountController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("ConStr");
        }

        public IActionResult Signup()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Signup(User user, string password)
        {
            var repo = new UserRepository(_connectionString);
            repo.AddUser(user, password);
            return Redirect("/account/login");
        }

        public IActionResult Login()
        {
            if (TempData["Error"] != null)
            {
                ViewBag.Message = TempData["Error"];
            }
            return View();
        }

        [HttpPost]
        public IActionResult Login(string email, string password)
        {
            var repo = new UserRepository(_connectionString);
            var user = repo.Login(email, password);
            if (user == null)
            {
                TempData["Error"] = "Invalid login!";
                return Redirect("/account/login");
            }

            //this code logs in the current user!
            var claims = new List<Claim>
            {
                new Claim("user", email)
            };

            HttpContext.SignInAsync(new ClaimsPrincipal(
                new ClaimsIdentity(claims, "Cookies", "user", "role"))).Wait();

            return Redirect("/");
        }

        public IActionResult CheckEmailAvailability(string email)
        {
            var repo = new UserRepository(_connectionString);
            bool isAvailable = repo.IsEmailAvailable(email);
            return Json(new EmailAvailabilityViewModel
            {
                IsAvailable = isAvailable
            });
        }

        public IActionResult Logout()
        {
            HttpContext.SignOutAsync().Wait();
            return Redirect("/");
        }
    }
}
