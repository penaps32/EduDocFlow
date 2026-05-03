using EduDocFlow.Web.Data;
using EduDocFlow.Web.Models;
using EduDocFlow.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace EduDocFlow.Web.Controllers
{
    [Authorize(Roles = nameof(UserRole.Methodist))]
    public class MethodistController(ApplicationDbContext context) : Controller
    {
        private readonly ApplicationDbContext _context = context;

        public async Task<IActionResult> Index()
        {

            var userIdValue = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!int.TryParse(userIdValue, out var userId))
            {
                return RedirectToAction("Login", "Account");
            }

            var currentUser = await _context.Users
                .FirstOrDefaultAsync(x => x.Id == userId);

            if (currentUser == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var requestsQuery = _context.DocumentRequests
                .Include(x => x.DocumentType)
                .Include(x => x.Student)
                    .ThenInclude(x => x.StudentProfile)
                .OrderByDescending(x => x.CreatedAt);

            var requests = await requestsQuery
                .Take(20)
                .Select(x => new MethodistRequestListItemViewModel
                {
                    Id = x.Id,
                    Number = x.Number,
                    StudentFullName = x.Student != null ? x.Student.FullName : "Студент не указан",
                    StudentGroup = x.Student != null && x.Student.StudentProfile != null
                        ? x.Student.StudentProfile.GroupName
                        : "Группа не указана",
                    DocumentTypeName = x.DocumentType != null ? x.DocumentType.Name : "Документ не указан",
                    Status = x.Status,
                    CreatedAt = x.CreatedAt,
                    DueDate = x.DueDate
                })
                .ToListAsync();

            var model = new MethodistDashboardViewModel
            {
                FullName = currentUser.FullName,

                TotalRequests = await _context.DocumentRequests.CountAsync(),

                CreatedRequests = await _context.DocumentRequests
        .CountAsync(x => x.Status == DocumentStatus.Created),

                InProgressRequests = await _context.DocumentRequests
        .CountAsync(x =>
            x.Status == DocumentStatus.OnTeacherReview ||
            x.Status == DocumentStatus.OnMethodistReview),

                CompletedRequests = await _context.DocumentRequests
        .CountAsync(x => x.Status == DocumentStatus.Completed),

                Requests = requests
            };

            return View("/Views/Methodist/Index.cshtml", model);


        }
    }
}