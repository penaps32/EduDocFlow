using System.ComponentModel.DataAnnotations;

namespace EduDocFlow.Web.ViewModels
{
	public class LoginViewModel
	{
		[Required(ErrorMessage = "Введите электронную почту")]
		[EmailAddress(ErrorMessage = "Введите корректный адрес электронной почты")]
		[Display(Name = "Электронная почта")]
		public string Email { get; set; } = string.Empty;

		[Required(ErrorMessage = "Введите пароль")]
		[DataType(DataType.Password)]
		[Display(Name = "Пароль")]
		public string Password { get; set; } = string.Empty;

		[Display(Name = "Запомнить вход")]
		public bool RememberMe { get; set; }
	}
}