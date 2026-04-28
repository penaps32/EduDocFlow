using System.ComponentModel.DataAnnotations;

namespace EduDocFlow.Web.Models
{
    public class RequestStatusHistory
    {
        public int Id { get; set; }

        [Display(Name = "Заявка")]
        public int DocumentRequestId { get; set; }

        public DocumentRequest? DocumentRequest { get; set; }

        [Display(Name = "Предыдущий статус")]
        public DocumentStatus? OldStatus { get; set; }

        [Display(Name = "Новый статус")]
        public DocumentStatus NewStatus { get; set; }

        [Display(Name = "Пользователь")]
        public int ChangedByUserId { get; set; }

        public User? ChangedByUser { get; set; }

        [StringLength(1000)]
        [Display(Name = "Комментарий")]
        public string Comment { get; set; } = string.Empty;

        [Display(Name = "Дата изменения")]
        public DateTime ChangedAt { get; set; } = DateTime.Now;
    }
}
