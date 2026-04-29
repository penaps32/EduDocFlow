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
        [Display(Name = "Код студента")]
        public string StudentCode { get; set; } = string.Empty;

        [Required]
        [StringLength(30)]
        [Display(Name = "Группа")]
        public string GroupName { get; set; } = string.Empty;

        [StringLength(200)]
        [Display(Name = "Образовательная программа")]
        public string EducationProgram { get; set; } = string.Empty;

        [Display(Name = "Курс")]
        public int Course { get; set; }

        [StringLength(50)]
        [Display(Name = "Форма обучения")]
        public string StudyForm { get; set; } = "очная";

        [Display(Name = "Дата зачисления")]
        public DateTime EnrollmentDate { get; set; }

        [Display(Name = "Проживает в общежитии")]
        public bool IsDormitoryResident { get; set; }

        [StringLength(50)]
        [Display(Name = "Статус студента")]
        public string StudentStatus { get; set; } = "обучается";
    }
}