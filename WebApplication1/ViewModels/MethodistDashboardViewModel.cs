using EduDocFlow.Web.Models;

namespace EduDocFlow.Web.ViewModels
{
    public class MethodistDashboardViewModel
    {
        public string FullName { get; set; } = string.Empty;

        public int TotalRequests { get; set; }

        public int CreatedRequests { get; set; }

        public int InProgressRequests { get; set; }

        public int CompletedRequests { get; set; }

        public List<MethodistRequestListItemViewModel> Requests { get; set; } = new();
    }

    public class MethodistRequestListItemViewModel
    {
        public int Id { get; set; }

        public string Number { get; set; } = string.Empty;

        public string StudentFullName { get; set; } = string.Empty;

        public string StudentGroup { get; set; } = string.Empty;

        public string DocumentTypeName { get; set; } = string.Empty;

        public DocumentStatus Status { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? DueDate { get; set; }
    }
}