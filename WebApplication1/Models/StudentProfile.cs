using System.ComponentModel.DataAnnotations;

namespace EduDocFlow.Web.Models
{
    public class StudentProfile
    {
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }

        public User? User { get; set; }

        [Required]
        [StringLength(30)]
        [Display(Name = "Группа")]
        public string GroupName { get; set; } = string.Empty;

        [StringLength(150)]
        [Display(Name = "Специальность")]
        public string Specialty { get; set; } = string.Empty;

        [StringLength(30)]
        [Display(Name = "Курс")]
        public string Course { get; set; } = string.Empty;

        [StringLength(50)]
        [Display(Name = "Форма обучения")]
        public string EducationForm { get; set; } = "очная";

        [StringLength(50)]
        [Display(Name = "Статус студента")]
        public string StudentStatus { get; set; } = "обучается";
    }
}
