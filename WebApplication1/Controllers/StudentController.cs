using System.Security.Claims;
using EduDocFlow.Web.Data;
using EduDocFlow.Web.Models;
using EduDocFlow.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;

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

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var model = new CreateDocumentRequestViewModel
            {
                DocumentTypes = await GetDocumentTypeItemsAsync()
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateDocumentRequestViewModel model)
        {
            var userIdValue = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!int.TryParse(userIdValue, out var userId))
            {
                return RedirectToAction("Login", "Account");
            }

            var studentProfile = await _context.StudentProfiles
                .FirstOrDefaultAsync(x => x.UserId == userId);

            if (studentProfile == null)
            {
                ModelState.AddModelError(string.Empty, "Профиль студента не найден. Обратитесь в учебную часть.");
                model.DocumentTypes = await GetDocumentTypeItemsAsync();
                return View(model);
            }

            if (!model.DocumentTypeId.HasValue)
            {
                ModelState.AddModelError(nameof(model.DocumentTypeId), "Выберите тип документа.");
                model.DocumentTypes = await GetDocumentTypeItemsAsync();
                return View(model);
            }

            var documentType = await _context.DocumentTypes
                .FirstOrDefaultAsync(x => x.Id == model.DocumentTypeId.Value && x.IsActive);

            if (documentType == null)
            {
                ModelState.AddModelError(nameof(model.DocumentTypeId), "Выбранный тип документа не найден.");
                model.DocumentTypes = await GetDocumentTypeItemsAsync();
                return View(model);
            }

            if (documentType.IsRecipientRequired && string.IsNullOrWhiteSpace(model.RecipientOrganization))
            {
                ModelState.AddModelError(nameof(model.RecipientOrganization), "Укажите получателя документа.");
            }

            if (string.IsNullOrWhiteSpace(model.Purpose))
            {
                ModelState.AddModelError(nameof(model.Purpose), "Укажите цель получения документа.");
            }

            if (documentType.IsWorkplaceRequired && string.IsNullOrWhiteSpace(model.Workplace))
            {
                ModelState.AddModelError(nameof(model.Workplace), "Укажите место работы.");
            }

            if (!ModelState.IsValid)
            {
                model.DocumentTypes = await GetDocumentTypeItemsAsync();
                return View(model);
            }

            var request = new DocumentRequest
            {
                Number = GenerateRequestNumber(),
                StudentId = userId,
                DocumentTypeId = model.DocumentTypeId.Value,
                Status = DocumentStatus.Created,
                RecipientOrganization = model.RecipientOrganization?.Trim() ?? string.Empty,
                DestinationPlace = model.DestinationPlace?.Trim() ?? string.Empty,
                Purpose = model.Purpose?.Trim() ?? string.Empty,
                StudyPeriodStart = studentProfile.EnrollmentDate,
                StudyPeriodEnd = null,
                Workplace = model.Workplace?.Trim() ?? string.Empty,
                StudentComment = model.StudentComment?.Trim() ?? string.Empty,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                DueDate = DateTime.Now.AddDays(5)
            };

            request.StatusHistoryItems.Add(new RequestStatusHistory
            {
                OldStatus = null,
                NewStatus = DocumentStatus.Created,
                ChangedByUserId = userId,
                Comment = "Заявка создана студентом.",
                ChangedAt = DateTime.Now
            });

            _context.DocumentRequests.Add(request);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Заявка успешно создана.";

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var userIdValue = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!int.TryParse(userIdValue, out var userId))
            {
                return RedirectToAction("Login", "Account");
            }

            var request = await _context.DocumentRequests
                .Include(x => x.DocumentType)
                .Include(x => x.Student)
                .Include(x => x.AssignedEmployee)
                .Include(x => x.StatusHistoryItems)
                    .ThenInclude(x => x.ChangedByUser)
                .FirstOrDefaultAsync(x => x.Id == id && x.StudentId == userId);

            if (request == null)
            {
                return NotFound();
            }

            return View(request);
        }

        private async Task<List<SelectListItem>> GetDocumentTypeItemsAsync()
        {
            return await _context.DocumentTypes
                .Where(x => x.IsActive)
                .OrderBy(x => x.SortOrder)
                .Select(x => new SelectListItem
                {
                    Value = x.Id.ToString(),
                    Text = x.Name
                })
                .ToListAsync();
        }

        private static string GenerateRequestNumber()
        {
            return $"REQ-{DateTime.Now:yyyyMMddHHmmss}-{Random.Shared.Next(1000, 9999)}";
        }

    }
}