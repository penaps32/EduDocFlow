using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace EduDocFlow.Web.ViewModels
{
    public class CreateDocumentRequestViewModel
    {
        [Required(ErrorMessage = "Выберите тип документа.")]
        [Display(Name = "Тип документа")]
        public int? DocumentTypeId { get; set; }

        public List<SelectListItem> DocumentTypes { get; set; } = new();

        [StringLength(300, ErrorMessage = "Получатель документа не должен превышать 300 символов.")]
        [Display(Name = "Получатель документа")]
        public string? RecipientOrganization { get; set; }

        [StringLength(300, ErrorMessage = "Дополнительные сведения не должны превышать 300 символов.")]
        [Display(Name = "Дополнительные сведения")]
        public string? DestinationPlace { get; set; }

        [StringLength(1000, ErrorMessage = "Цель получения не должна превышать 1000 символов.")]
        [Display(Name = "Цель получения")]
        public string? Purpose { get; set; }

        [StringLength(300, ErrorMessage = "Место работы не должно превышать 300 символов.")]
        [Display(Name = "Место работы")]
        public string? Workplace { get; set; }

        [StringLength(1000, ErrorMessage = "Комментарий к заявке не должен превышать 1000 символов.")]
        [Display(Name = "Комментарий к заявке")]
        public string? StudentComment { get; set; }
    }
}