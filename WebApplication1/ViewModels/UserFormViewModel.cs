using EduDocFlow.Web.Models;
using System.ComponentModel.DataAnnotations;

namespace EduDocFlow.Web.ViewModels
{
    public class UserFormViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Введите ФИО пользователя.")]
        [StringLength(150, ErrorMessage = "ФИО не должно быть длиннее 150 символов.")]
        [Display(Name = "ФИО")]
        public string FullName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Введите электронную почту.")]
        [EmailAddress(ErrorMessage = "Введите корректную электронную почту.")]
        [StringLength(120, ErrorMessage = "Электронная почта не должна быть длиннее 120 символов.")]
        [Display(Name = "Электронная почта")]
        public string Email { get; set; } = string.Empty;

        [Display(Name = "Пароль")]
        public string? Password { get; set; }

        [Display(Name = "Роль")]
        public UserRole Role { get; set; } = UserRole.Student;

        [Display(Name = "Активен")]
        public bool IsActive { get; set; } = true;

        [Display(Name = "Код студента")]
        public string StudentCode { get; set; } = string.Empty;

        [Display(Name = "Группа")]
        public string GroupName { get; set; } = string.Empty;

        [Display(Name = "Образовательная программа")]
        public string EducationProgram { get; set; } = string.Empty;

        [Display(Name = "Курс")]
        public int Course { get; set; } = 1;

        [Display(Name = "Форма обучения")]
        public string StudyForm { get; set; } = "очная";

        [Display(Name = "Дата зачисления")]
        public DateTime EnrollmentDate { get; set; } = DateTime.Today;

        [Display(Name = "Проживает в общежитии")]
        public bool IsDormitoryResident { get; set; }

        [Display(Name = "Статус студента")]
        public string StudentStatus { get; set; } = "обучается";
    }
}