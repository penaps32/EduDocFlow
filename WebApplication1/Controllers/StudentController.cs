using System.Security.Claims;
using EduDocFlow.Web.Data;
using EduDocFlow.Web.Models;
using EduDocFlow.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EduDocFlow.Web.Controllers
{
    [Authorize(Roles = "Student")]
    public class StudentController(ApplicationDbContext context) : Controller
    {
        private readonly ApplicationDbContext _context = context;

        public async Task<IActionResult> Index()
        {
            var userIdValue = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!int.TryParse(userIdValue, out var userId))
            {
                return RedirectToAction("Login", "Account");
            }

            var user = await _context.Users
                .Include(x => x.StudentProfile)
                .FirstOrDefaultAsync(x => x.Id == userId);

            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var requestsQuery = _context.DocumentRequests
                .Include(x => x.DocumentType)
                .Where(x => x.StudentId == userId);

            var model = new StudentDashboardViewModel
            {
                FullName = user.FullName,
                Email = user.Email,
                Profile = user.StudentProfile,

                TotalRequests = await requestsQuery.CountAsync(),

                CreatedRequests = await requestsQuery
                    .CountAsync(x => x.Status == DocumentStatus.Created),

                InProgressRequests = await requestsQuery
                    .CountAsync(x =>
                        x.Status == DocumentStatus.OnTeacherReview ||
                        x.Status == DocumentStatus.OnMethodistReview),

                CompletedRequests = await requestsQuery
                    .CountAsync(x => x.Status == DocumentStatus.Completed),

                RejectedRequests = await requestsQuery
                    .CountAsync(x => x.Status == DocumentStatus.Rejected),

                RecentRequests = await requestsQuery
                    .OrderByDescending(x => x.CreatedAt)
                    .Take(5)
                    .ToListAsync()
            };

            return View(model);
        }
    }
}