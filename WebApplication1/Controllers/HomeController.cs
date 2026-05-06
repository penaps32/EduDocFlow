using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using EduDocFlow.Web.Models;

namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            if (User.Identity?.IsAuthenticated == true)
            {
                if (User.IsInRole(nameof(UserRole.Student)))
                {
                    return RedirectToAction("Index", "Student");
                }

                if (User.IsInRole(nameof(UserRole.Methodist)))
                {
                    return RedirectToAction("Index", "Methodist");
                }

                if (User.IsInRole(nameof(UserRole.Admin)))
                {
                    return RedirectToAction("Index", "Admin");
                }
            }

            return View();
        }

        public IActionResult Privacy()
        {
            return RedirectToAction(nameof(Index));
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel
            {
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
            });
        }
    }
}