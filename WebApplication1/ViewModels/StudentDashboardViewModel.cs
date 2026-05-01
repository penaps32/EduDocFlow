using EduDocFlow.Web.Models;

namespace EduDocFlow.Web.ViewModels
{
    public class StudentDashboardViewModel
    {
        public string FullName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public StudentProfile? Profile { get; set; }

        public int TotalRequests { get; set; }

        public int CreatedRequests { get; set; }

        public int InProgressRequests { get; set; }

        public int CompletedRequests { get; set; }

        public int RejectedRequests { get; set; }

        public List<DocumentRequest> RecentRequests { get; set; } = new();
    }
}