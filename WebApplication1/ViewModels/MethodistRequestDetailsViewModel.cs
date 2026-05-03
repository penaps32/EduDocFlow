using EduDocFlow.Web.Models;

namespace EduDocFlow.Web.ViewModels
{
    public class MethodistRequestDetailsViewModel
    {
        public int Id { get; set; }

        public string Number { get; set; } = string.Empty;

        public string StudentFullName { get; set; } = string.Empty;

        public string StudentEmail { get; set; } = string.Empty;

        public string StudentGroup { get; set; } = string.Empty;

        public string EducationProgram { get; set; } = string.Empty;

        public int Course { get; set; }

        public string StudyForm { get; set; } = string.Empty;

        public string DocumentTypeName { get; set; } = string.Empty;

        public DocumentStatus Status { get; set; }

        public string RecipientOrganization { get; set; } = string.Empty;

        public string DestinationPlace { get; set; } = string.Empty;

        public string Purpose { get; set; } = string.Empty;

        public string Workplace { get; set; } = string.Empty;

        public string StudentComment { get; set; } = string.Empty;

        public DateTime? StudyPeriodStart { get; set; }

        public DateTime? StudyPeriodEnd { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        public DateTime? DueDate { get; set; }

        public List<MethodistRequestHistoryItemViewModel> HistoryItems { get; set; } = new();

        public List<MethodistRequestCommentViewModel> CommentItems { get; set; } = new();
    }

    public class MethodistRequestHistoryItemViewModel
    {
        public DocumentStatus NewStatus { get; set; }

        public string Comment { get; set; } = string.Empty;

        public DateTime ChangedAt { get; set; }

        public string ChangedByUserName { get; set; } = string.Empty;
    }

    public class MethodistRequestCommentViewModel
    {
        public string AuthorName { get; set; } = string.Empty;

        public string Text { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; }
    }

    public class ChangeRequestStatusViewModel
    {
        public int RequestId { get; set; }

        public DocumentStatus NewStatus { get; set; }

        public string? Comment { get; set; }
    }
}