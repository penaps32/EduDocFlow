using System.ComponentModel.DataAnnotations;

namespace EduDocFlow.Web.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required]
        [StringLength(150)]
        [Display(Name = "ФИО")]
        public string FullName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [StringLength(120)]
        [Display(Name = "Электронная почта")]
        public string Email { get; set; } = string.Empty;

        [Required]
        [StringLength(255)]
        public string PasswordHash { get; set; } = string.Empty;

        [Display(Name = "Роль")]
        public UserRole Role { get; set; } = UserRole.Student;

        [Display(Name = "Активен")]
        public bool IsActive { get; set; } = true;

        [Display(Name = "Дата создания")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public StudentProfile? StudentProfile { get; set; }

        public ICollection<DocumentRequest> CreatedRequests { get; set; } = new List<DocumentRequest>();

        public ICollection<DocumentRequest> AssignedRequests { get; set; } = new List<DocumentRequest>();

        public ICollection<RequestStatusHistory> StatusHistoryItems { get; set; } = new List<RequestStatusHistory>();

        public ICollection<RequestComment> Comments { get; set; } = new List<RequestComment>();
    }
}
