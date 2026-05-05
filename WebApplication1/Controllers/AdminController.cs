using EduDocFlow.Web.Data;
using EduDocFlow.Web.Models;
using EduDocFlow.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;

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

        [HttpGet]
        public IActionResult CreateUser()
        {
            var model = new UserFormViewModel
            {
                IsActive = true,
                Role = UserRole.Student,
                EnrollmentDate = DateTime.Today,
                StudyForm = "очная",
                StudentStatus = "обучается"
            };

            return View("/Views/Admin/UserForm.cshtml", model);
        }

        [HttpGet]
        public async Task<IActionResult> EditUser(int id)
        {
            var user = await _context.Users
                .Include(x => x.StudentProfile)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (user == null)
            {
                return NotFound();
            }

            var model = new UserFormViewModel
            {
                Id = user.Id,
                FullName = user.FullName,
                Email = user.Email,
                Role = user.Role,
                IsActive = user.IsActive,

                StudentCode = user.StudentProfile?.StudentCode ?? string.Empty,
                GroupName = user.StudentProfile?.GroupName ?? string.Empty,
                EducationProgram = user.StudentProfile?.EducationProgram ?? string.Empty,
                Course = user.StudentProfile?.Course ?? 1,
                StudyForm = user.StudentProfile?.StudyForm ?? "очная",
                EnrollmentDate = user.StudentProfile?.EnrollmentDate ?? DateTime.Today,
                IsDormitoryResident = user.StudentProfile?.IsDormitoryResident ?? false,
                StudentStatus = user.StudentProfile?.StudentStatus ?? "обучается"
            };

            return View("/Views/Admin/UserForm.cshtml", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveUser(UserFormViewModel model)
        {
            if (model.Id == 0 && string.IsNullOrWhiteSpace(model.Password))
            {
                ModelState.AddModelError(nameof(model.Password), "Введите пароль для нового пользователя.");
            }

            if (model.Role == UserRole.Student)
            {
                if (string.IsNullOrWhiteSpace(model.StudentCode))
                    ModelState.AddModelError(nameof(model.StudentCode), "Введите код студента.");

                if (string.IsNullOrWhiteSpace(model.GroupName))
                    ModelState.AddModelError(nameof(model.GroupName), "Введите группу студента.");

                if (model.Course <= 0)
                    ModelState.AddModelError(nameof(model.Course), "Введите корректный курс.");
            }

            if (!ModelState.IsValid)
            {
                return View("/Views/Admin/UserForm.cshtml", model);
            }

            var normalizedEmail = model.Email.Trim().ToLower();

            var emailExists = await _context.Users
                .AnyAsync(x => x.Email == normalizedEmail && x.Id != model.Id);

            if (emailExists)
            {
                ModelState.AddModelError(nameof(model.Email), "Пользователь с такой электронной почтой уже существует.");
                return View("/Views/Admin/UserForm.cshtml", model);
            }

            User user;

            if (model.Id == 0)
            {
                user = new User
                {
                    CreatedAt = DateTime.Now
                };

                _context.Users.Add(user);
            }
            else
            {
                user = await _context.Users
                    .Include(x => x.StudentProfile)
                    .FirstOrDefaultAsync(x => x.Id == model.Id);

                if (user == null)
                {
                    return NotFound();
                }
            }

            user.FullName = model.FullName.Trim();
            user.Email = normalizedEmail;
            user.Role = model.Role;
            user.IsActive = model.IsActive;

            if (!string.IsNullOrWhiteSpace(model.Password))
            {
                var passwordHasher = new PasswordHasher<User>();
                user.PasswordHash = passwordHasher.HashPassword(user, model.Password);
            }

            if (model.Role == UserRole.Student)
            {
                if (user.StudentProfile == null)
                {
                    user.StudentProfile = new StudentProfile();
                }

                user.StudentProfile.StudentCode = model.StudentCode.Trim();
                user.StudentProfile.GroupName = model.GroupName.Trim();
                user.StudentProfile.EducationProgram = model.EducationProgram?.Trim() ?? string.Empty;
                user.StudentProfile.Course = model.Course;
                user.StudentProfile.StudyForm = model.StudyForm?.Trim() ?? "очная";
                user.StudentProfile.EnrollmentDate = model.EnrollmentDate;
                user.StudentProfile.IsDormitoryResident = model.IsDormitoryResident;
                user.StudentProfile.StudentStatus = model.StudentStatus?.Trim() ?? "обучается";
            }

            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = model.Id == 0
                ? "Пользователь добавлен."
                : "Пользователь обновлен.";

            return RedirectToAction(nameof(Index));
        }

    }
}
