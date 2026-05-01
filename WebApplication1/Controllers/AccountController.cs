using System.Security.Claims;
using EduDocFlow.Web.Data;
using EduDocFlow.Web.Models;
using EduDocFlow.Web.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EduDocFlow.Web.Controllers
{
    public class AccountController(ApplicationDbContext context) : Controller
    {
        private readonly ApplicationDbContext _context = context;

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login()
        {
            if (User.Identity?.IsAuthenticated == true)
            {
                if (User.IsInRole(UserRole.Student.ToString()))
                    return RedirectToAction("Index", "Student");

                if (User.IsInRole(UserRole.Methodist.ToString()))
                    return RedirectToAction("Index", "Methodist");

                if (User.IsInRole(UserRole.Admin.ToString()))
                    return RedirectToAction("Index", "Admin");

                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        private RedirectToActionResult RedirectByRole(UserRole role)
        {
            return role switch
            {
                UserRole.Student => RedirectToAction("Index", "Student"),
                UserRole.Methodist => RedirectToAction("Index", "Methodist"),
                UserRole.Admin => RedirectToAction("Index", "Admin"),
                _ => RedirectToAction("Index", "Home")
            };
        }


        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var email = model.Email.Trim().ToLower();

            var user = await _context.Users
                .FirstOrDefaultAsync(x => x.IsActive && x.Email == email);

            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "Неверная электронная почта или пароль");
                return View(model);
            }

            var passwordHasher = new PasswordHasher<User>();
            var result = passwordHasher.VerifyHashedPassword(user, user.PasswordHash, model.Password);

            if (result == PasswordVerificationResult.Failed)
            {
                ModelState.AddModelError(string.Empty, "Неверная электронная почта или пароль");
                return View(model);
            }

            var claims = new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new(ClaimTypes.Name, user.FullName),
                new(ClaimTypes.Email, user.Email),
                new(ClaimTypes.Role, user.Role.ToString())
            };

            var claimsIdentity = new ClaimsIdentity(
                claims,
                CookieAuthenticationDefaults.AuthenticationScheme);

            var authProperties = new AuthenticationProperties
            {
                IsPersistent = model.RememberMe
            };

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);

            return RedirectByRole(user.Role);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Account");
        }

        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }


    }
}