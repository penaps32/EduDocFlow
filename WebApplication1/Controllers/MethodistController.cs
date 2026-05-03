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
    public class MethodistController(ApplicationDbContext context, IConfiguration configuration) : Controller
    {
        private readonly ApplicationDbContext _context = context;
        private readonly IConfiguration _configuration = configuration;

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

            var requests = await _context.DocumentRequests
                .Include(x => x.DocumentType)
                .Include(x => x.Student!)
                    .ThenInclude(x => x.StudentProfile)
                .OrderByDescending(x => x.CreatedAt)
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

            var teacherReviewRequests = await _context.DocumentRequests
                .CountAsync(x => x.Status == DocumentStatus.OnTeacherReview);

            var methodistReviewRequests = await _context.DocumentRequests
                .CountAsync(x => x.Status == DocumentStatus.OnMethodistReview);

            var model = new MethodistDashboardViewModel
            {
                FullName = currentUser.FullName,

                TotalRequests = await _context.DocumentRequests.CountAsync(),

                CreatedRequests = await _context.DocumentRequests
                    .CountAsync(x => x.Status == DocumentStatus.Created),

                TeacherReviewRequests = teacherReviewRequests,

                MethodistReviewRequests = methodistReviewRequests,

                InProgressRequests = teacherReviewRequests + methodistReviewRequests,

                CompletedRequests = await _context.DocumentRequests
                    .CountAsync(x => x.Status == DocumentStatus.Completed),

                RejectedRequests = await _context.DocumentRequests
                    .CountAsync(x => x.Status == DocumentStatus.Rejected),

                Requests = requests
            };

            return View("/Views/Methodist/Index.cshtml", model);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var request = await _context.DocumentRequests
                .Include(x => x.DocumentType)
                .Include(x => x.Student!)
                    .ThenInclude(x => x.StudentProfile)
                .Include(x => x.AssignedEmployee)
                .Include(x => x.StatusHistoryItems)
                    .ThenInclude(x => x.ChangedByUser)
                .Include(x => x.Comments)
                    .ThenInclude(x => x.Author)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (request == null)
            {
                return NotFound();
            }

            var model = new MethodistRequestDetailsViewModel
            {
                Id = request.Id,
                Number = request.Number,
                StudentFullName = request.Student?.FullName ?? "Студент не указан",
                StudentEmail = request.Student?.Email ?? "Почта не указана",
                StudentGroup = request.Student?.StudentProfile?.GroupName ?? "Группа не указана",
                EducationProgram = request.Student?.StudentProfile?.EducationProgram ?? "Не указана",
                Course = request.Student?.StudentProfile?.Course ?? 0,
                StudyForm = request.Student?.StudentProfile?.StudyForm ?? "Не указана",
                DocumentTypeName = request.DocumentType?.Name ?? "Документ не указан",
                Status = request.Status,
                RecipientOrganization = request.RecipientOrganization,
                DestinationPlace = request.DestinationPlace,
                Purpose = request.Purpose,
                Workplace = request.Workplace,
                StudentComment = request.StudentComment,
                StudyPeriodStart = request.StudyPeriodStart,
                StudyPeriodEnd = request.StudyPeriodEnd,
                CreatedAt = request.CreatedAt,
                UpdatedAt = request.UpdatedAt ?? request.CreatedAt,
                DueDate = request.DueDate,

                HistoryItems = request.StatusHistoryItems
                    .OrderByDescending(x => x.ChangedAt)
                    .Select(x => new MethodistRequestHistoryItemViewModel
                    {
                        NewStatus = x.NewStatus,
                        Comment = x.Comment,
                        ChangedAt = x.ChangedAt,
                        ChangedByUserName = x.ChangedByUser != null
                            ? x.ChangedByUser.FullName
                            : "Пользователь не указан"
                    })
                    .ToList(),

                CommentItems = request.Comments
                    .OrderByDescending(x => x.CreatedAt)
                    .Select(x => new MethodistRequestCommentViewModel
                    {
                        AuthorName = x.Author != null ? x.Author.FullName : "Автор не указан",
                        Text = x.Text,
                        CreatedAt = x.CreatedAt
                    })
                    .ToList()
            };

            return View("/Views/Methodist/Details.cshtml", model);
        }

        [HttpGet]
        public async Task<IActionResult> PrintDocument(int id)
        {
            var request = await _context.DocumentRequests
                .Include(x => x.DocumentType)
                .Include(x => x.Student)
                    .ThenInclude(x => x.StudentProfile)
                .Include(x => x.AssignedEmployee)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (request == null)
            {
                return NotFound();
            }

            if (request.Status != DocumentStatus.Completed)
            {
                TempData["ErrorMessage"] = "Документ можно сформировать только после перевода заявки в статус «Готова».";
                return RedirectToAction(nameof(Details), new { id = request.Id });
            }
            var organizationSection = _configuration.GetSection("Organization");

            ViewData["OrganizationNameLine1"] = organizationSection["NameLine1"] ?? "Образовательная организация";
            ViewData["OrganizationNameLine2"] = organizationSection["NameLine2"] ?? "среднего профессионального образования";
            ViewData["OrganizationNameLine3"] = organizationSection["NameLine3"] ?? "«Учебный колледж»";
            ViewData["OrganizationInn"] = organizationSection["Inn"] ?? "0000000000";
            ViewData["OrganizationKpp"] = organizationSection["Kpp"] ?? "000000000";
            ViewData["OrganizationAddress"] = organizationSection["Address"] ?? "000000, г. Город, ул. Учебная, д. 1";
            ViewData["OrganizationPhone"] = organizationSection["Phone"] ?? "+7 (000) 000-00-00";
            ViewData["OrganizationEmail"] = organizationSection["Email"] ?? "info@college.local";
            ViewData["OrganizationWebsite"] = organizationSection["Website"] ?? "www.college.local";
            ViewData["DirectorPosition"] = organizationSection["DirectorPosition"] ?? "Директор";
            ViewData["DirectorName"] = organizationSection["DirectorName"] ?? "И.И. Иванова";
            ViewData["ExecutorName"] = organizationSection["ExecutorName"] ?? "специалист учебного отдела";
            ViewData["ExecutorPhone"] = organizationSection["ExecutorPhone"] ?? "+7 (000) 000-00-00";
            return View("/Views/Methodist/PrintDocument.cshtml", request);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangeStatus(ChangeRequestStatusViewModel model)
        {
            var userIdValue = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!int.TryParse(userIdValue, out var userId))
            {
                return RedirectToAction("Login", "Account");
            }

            var request = await _context.DocumentRequests
                .Include(x => x.StatusHistoryItems)
                .Include(x => x.Comments)
                .FirstOrDefaultAsync(x => x.Id == model.RequestId);

            if (request == null)
            {
                return NotFound();
            }

            var oldStatus = request.Status;
            var newStatus = model.NewStatus;
            var now = DateTime.Now;
            var comment = model.Comment?.Trim() ?? string.Empty;

            if (newStatus == DocumentStatus.Rejected && string.IsNullOrWhiteSpace(comment))
            {
                TempData["ErrorMessage"] = "При отклонении заявки нужно указать причину.";
                return RedirectToAction(nameof(Details), new { id = request.Id });
            }

            if (oldStatus == newStatus && string.IsNullOrWhiteSpace(comment))
            {
                TempData["ErrorMessage"] = "Статус не изменен. Выберите другой статус или добавьте комментарий.";
                return RedirectToAction(nameof(Details), new { id = request.Id });
            }

            var statusChanged = oldStatus != newStatus;

            request.UpdatedAt = now;
            request.AssignedEmployeeId = userId;

            if (statusChanged)
            {
                request.Status = newStatus;
                request.EmployeeComment = comment;

                if (newStatus == DocumentStatus.Completed)
                {
                    request.CompletedAt = now;
                }
                else
                {
                    request.CompletedAt = null;
                }

                request.StatusHistoryItems.Add(new RequestStatusHistory
                {
                    DocumentRequestId = request.Id,
                    OldStatus = oldStatus,
                    NewStatus = newStatus,
                    ChangedByUserId = userId,
                    Comment = string.IsNullOrWhiteSpace(comment)
                        ? "Статус заявки изменен сотрудником учебной части."
                        : comment,
                    ChangedAt = now
                });
            }

            if (!string.IsNullOrWhiteSpace(comment))
            {
                request.Comments.Add(new RequestComment
                {
                    DocumentRequestId = request.Id,
                    AuthorId = userId,
                    Text = comment,
                    CreatedAt = now
                });
            }

            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = statusChanged
                ? "Статус заявки обновлен."
                : "Комментарий добавлен.";

            return RedirectToAction(nameof(Details), new { id = request.Id });
        }
    }
}