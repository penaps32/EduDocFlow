using System.ComponentModel.DataAnnotations;

namespace EduDocFlow.Web.Models
{
    public class RequestComment
    {
        public int Id { get; set; }

        [Display(Name = "Заявка")]
        public int DocumentRequestId { get; set; }

        public DocumentRequest? DocumentRequest { get; set; }

        [Display(Name = "Автор")]
        public int AuthorId { get; set; }

        public User? Author { get; set; }

        [Required]
        [StringLength(1000)]
        [Display(Name = "Текст комментария")]
        public string Text { get; set; } = string.Empty;

        [Display(Name = "Дата создания")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
