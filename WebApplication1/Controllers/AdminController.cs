using EduDocFlow.Web.Data;
using EduDocFlow.Web.Models;
using EduDocFlow.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace EduDocFlow.Web.Controllers
{
    [Authorize(Roles = nameof(UserRole.Admin))]
    public class AdminController(ApplicationDbContext context) : Controller
    {
        private readonly ApplicationDbContext _context = context;

        public async Task<IActionResult> Index()
        {
            var userIdValue = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!int.TryParse(userIdValue, out var currentUserId))
            {
                return RedirectToAction("Login", "Account");
            }

            var inProgressRequests = await _context.DocumentRequests
                .CountAsync(x =>
                    x.Status == DocumentStatus.OnTeacherReview ||
                    x.Status == DocumentStatus.OnMethodistReview);

            var model = new AdminDashboardViewModel
            {
                CurrentUserId = currentUserId,

                TotalUsers = await _context.Users.CountAsync(),

                ActiveUsers = await _context.Users
                    .CountAsync(x => x.IsActive),

                StudentsCount = await _context.Users
                    .CountAsync(x => x.Role == UserRole.Student),

                MethodistsCount = await _context.Users
                    .CountAsync(x => x.Role == UserRole.Methodist),

                AdminsCount = await _context.Users
                    .CountAsync(x => x.Role == UserRole.Admin),

                DocumentTypesCount = await _context.DocumentTypes.CountAsync(),

                ActiveDocumentTypesCount = await _context.DocumentTypes
                    .CountAsync(x => x.IsActive),

                TotalRequests = await _context.DocumentRequests.CountAsync(),

                CreatedRequests = await _context.DocumentRequests
                    .CountAsync(x => x.Status == DocumentStatus.Created),

                InProgressRequests = inProgressRequests,

                CompletedRequests = await _context.DocumentRequests
                    .CountAsync(x => x.Status == DocumentStatus.Completed),

                RejectedRequests = await _context.DocumentRequests
                    .CountAsync(x => x.Status == DocumentStatus.Rejected),

                RecentUsers = await _context.Users
                    .OrderByDescending(x => x.CreatedAt)
                    .Take(10)
                    .Select(x => new AdminUserListItemViewModel
                    {
                        Id = x.Id,
                        FullName = x.FullName,
                        Email = x.Email,
                        Role = x.Role,
                        IsActive = x.IsActive,
                        CreatedAt = x.CreatedAt
                    })
                    .ToListAsync(),

                DocumentTypes = await _context.DocumentTypes
                    .OrderBy(x => x.SortOrder)
                    .ThenBy(x => x.Name)
                    .Select(x => new AdminDocumentTypeListItemViewModel
                    {
                        Id = x.Id,
                        Code = x.Code,
                        Name = x.Name,
                        Category = x.Category,
                        TemplateKind = x.TemplateKind,
                        IsActive = x.IsActive,
                        SortOrder = x.SortOrder
                    })
                    .ToListAsync()
            };

            return View("/Views/Admin/Index.cshtml", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleUserActive(int id)
        {
            var currentUserIdValue = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!int.TryParse(currentUserIdValue, out var currentUserId))
            {
                return RedirectToAction("Login", "Account");
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(x => x.Id == id);

            if (user == null)
            {
                return NotFound();
            }

            if (user.Id == currentUserId)
            {
                TempData["ErrorMessage"] = "Нельзя отключить собственную учетную запись.";
                return RedirectToAction(nameof(Index));
            }

            if (user.Role == UserRole.Admin && user.IsActive)
            {
                var activeAdminsCount = await _context.Users
                    .CountAsync(x => x.Role == UserRole.Admin && x.IsActive);

                if (activeAdminsCount <= 1)
                {
                    TempData["ErrorMessage"] = "Нельзя отключить последнего активного администратора.";
                    return RedirectToAction(nameof(Index));
                }
            }

            user.IsActive = !user.IsActive;

            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = user.IsActive
                ? "Пользователь включен."
                : "Пользователь отключен.";

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleDocumentTypeActive(int id)
        {
            var documentType = await _context.DocumentTypes
                .FirstOrDefaultAsync(x => x.Id == id);

            if (documentType == null)
            {
                return NotFound();
            }

            documentType.IsActive = !documentType.IsActive;

            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = documentType.IsActive
                ? "Тип документа включен."
                : "Тип документа отключен.";

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult CreateDocumentType()
        {
            var model = new DocumentTypeFormViewModel
            {
                IsActive = true,
                RequiresOriginalSignature = true
            };

            return View("/Views/Admin/DocumentTypeForm.cshtml", model);
        }

        [HttpGet]
        public async Task<IActionResult> EditDocumentType(int id)
        {
            var documentType = await _context.DocumentTypes
                .FirstOrDefaultAsync(x => x.Id == id);

            if (documentType == null)
            {
                return NotFound();
            }

            var model = new DocumentTypeFormViewModel
            {
                Id = documentType.Id,
                Code = documentType.Code,
                Name = documentType.Name,
                Category = documentType.Category,
                TemplateKind = documentType.TemplateKind,
                Description = documentType.Description,
                LegalBasis = documentType.LegalBasis,
                IsRecipientRequired = documentType.IsRecipientRequired,
                IsStudyPeriodRequired = documentType.IsStudyPeriodRequired,
                IsWorkplaceRequired = documentType.IsWorkplaceRequired,
                RequiresOriginalSignature = documentType.RequiresOriginalSignature,
                IsActive = documentType.IsActive,
                SortOrder = documentType.SortOrder
            };

            return View("/Views/Admin/DocumentTypeForm.cshtml", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveDocumentType(DocumentTypeFormViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("/Views/Admin/DocumentTypeForm.cshtml", model);
            }

            var normalizedCode = model.Code.Trim();

            var codeExists = await _context.DocumentTypes
                .AnyAsync(x => x.Code == normalizedCode && x.Id != model.Id);

            if (codeExists)
            {
                ModelState.AddModelError(nameof(model.Code), "Тип документа с таким кодом уже существует.");
                return View("/Views/Admin/DocumentTypeForm.cshtml", model);
            }

            DocumentType documentType;

            if (model.Id == 0)
            {
                documentType = new DocumentType();
                _context.DocumentTypes.Add(documentType);
            }
            else
            {
                documentType = await _context.DocumentTypes
                    .FirstOrDefaultAsync(x => x.Id == model.Id);

                if (documentType == null)
                {
                    return NotFound();
                }
            }

            documentType.Code = normalizedCode;
            documentType.Name = model.Name.Trim();
            documentType.Category = model.Category;
            documentType.TemplateKind = model.TemplateKind;
            documentType.Description = model.Description?.Trim() ?? string.Empty;
            documentType.LegalBasis = model.LegalBasis?.Trim() ?? string.Empty;
            documentType.IsRecipientRequired = model.IsRecipientRequired;
            documentType.IsStudyPeriodRequired = model.IsStudyPeriodRequired;
            documentType.IsWorkplaceRequired = model.IsWorkplaceRequired;
            documentType.RequiresOriginalSignature = model.RequiresOriginalSignature;
            documentType.IsActive = model.IsActive;
            documentType.SortOrder = model.SortOrder;

            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = model.Id == 0
                ? "Тип документа добавлен."
                : "Тип документа обновлен.";

            return RedirectToAction(nameof(Index));
        }

    }
}
