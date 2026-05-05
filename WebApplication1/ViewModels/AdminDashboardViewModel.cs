using EduDocFlow.Web.Models;

namespace EduDocFlow.Web.ViewModels
{
    public class AdminDashboardViewModel
    {
        public int CurrentUserId { get; set; }

        public int TotalUsers { get; set; }

        public int ActiveUsers { get; set; }

        public int StudentsCount { get; set; }

        public int MethodistsCount { get; set; }

        public int AdminsCount { get; set; }

        public int DocumentTypesCount { get; set; }

        public int ActiveDocumentTypesCount { get; set; }

        public int TotalRequests { get; set; }

        public int CreatedRequests { get; set; }

        public int InProgressRequests { get; set; }

        public int CompletedRequests { get; set; }

        public int RejectedRequests { get; set; }

        public List<AdminUserListItemViewModel> RecentUsers { get; set; } = new();

        public List<AdminDocumentTypeListItemViewModel> DocumentTypes { get; set; } = new();
    }

    public class AdminUserListItemViewModel
    {
        public int Id { get; set; }

        public string FullName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public UserRole Role { get; set; }

        public bool IsActive { get; set; }

        public DateTime CreatedAt { get; set; }
    }

    public class AdminDocumentTypeListItemViewModel
    {
        public int Id { get; set; }

        public string Code { get; set; } = string.Empty;

        public string Name { get; set; } = string.Empty;

        public DocumentTypeCategory Category { get; set; }

        public DocumentTemplateKind TemplateKind { get; set; }

        public bool IsActive { get; set; }

        public int SortOrder { get; set; }
    }
}