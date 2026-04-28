using System.ComponentModel.DataAnnotations;

namespace EduDocFlow.Web.Models
{
    public class DocumentRequest
    {
        public int Id { get; set; }

        [Required]
        [StringLength(40)]
        [Display(Name = "Номер заявки")]
        public string Number { get; set; } = string.Empty;

        [Display(Name = "Студент")]
        public int StudentId { get; set; }

        public User? Student { get; set; }

        [Display(Name = "Тип документа")]
        public int DocumentTypeId { get; set; }

        public DocumentType? DocumentType { get; set; }

        [Display(Name = "Ответственный сотрудник")]
        public int? AssignedEmployeeId { get; set; }

        public User? AssignedEmployee { get; set; }

        [Display(Name = "Статус")]
        public DocumentStatus Status { get; set; } = DocumentStatus.Created;

        [StringLength(300)]
        [Display(Name = "Получатель документа")]
        public string RecipientOrganization { get; set; } = string.Empty;

        [StringLength(300)]
        [Display(Name = "Место предоставления")]
        public string DestinationPlace { get; set; } = string.Empty;

        [StringLength(500)]
        [Display(Name = "Цель получения")]
        public string Purpose { get; set; } = string.Empty;

        [Display(Name = "Начало периода обучения")]
        public DateTime? StudyPeriodStart { get; set; }

        [Display(Name = "Окончание периода обучения")]
        public DateTime? StudyPeriodEnd { get; set; }

        [StringLength(300)]
        [Display(Name = "Место работы")]
        public string Workplace { get; set; } = string.Empty;

        [StringLength(1000)]
        [Display(Name = "Комментарий студента")]
        public string StudentComment { get; set; } = string.Empty;

        [StringLength(1000)]
        [Display(Name = "Комментарий сотрудника")]
        public string EmployeeComment { get; set; } = string.Empty;

        [Display(Name = "Дата создания")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [Display(Name = "Дата обновления")]
        public DateTime? UpdatedAt { get; set; }

        [Display(Name = "Срок исполнения")]
        public DateTime? DueDate { get; set; }

        [Display(Name = "Дата завершения")]
        public DateTime? CompletedAt { get; set; }

        public ICollection<RequestStatusHistory> StatusHistoryItems { get; set; } = new List<RequestStatusHistory>();

        public ICollection<RequestComment> Comments { get; set; } = new List<RequestComment>();
    }
}
