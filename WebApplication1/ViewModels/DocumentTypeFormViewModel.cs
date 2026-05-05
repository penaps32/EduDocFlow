using EduDocFlow.Web.Models;
using System.ComponentModel.DataAnnotations;

namespace EduDocFlow.Web.ViewModels
{
    public class DocumentTypeFormViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Введите код документа.")]
        [StringLength(64, ErrorMessage = "Код документа не должен быть длиннее 64 символов.")]
        [Display(Name = "Код документа")]
        public string Code { get; set; } = string.Empty;

        [Required(ErrorMessage = "Введите наименование документа.")]
        [StringLength(200, ErrorMessage = "Наименование документа не должно быть длиннее 200 символов.")]
        [Display(Name = "Наименование документа")]
        public string Name { get; set; } = string.Empty;

        [Display(Name = "Категория")]
        public DocumentTypeCategory Category { get; set; } = DocumentTypeCategory.Certificate;

        [Display(Name = "Вид шаблона")]
        public DocumentTemplateKind TemplateKind { get; set; } = DocumentTemplateKind.None;

        [StringLength(1000, ErrorMessage = "Описание не должно быть длиннее 1000 символов.")]
        [Display(Name = "Описание")]
        public string Description { get; set; } = string.Empty;

        [StringLength(700, ErrorMessage = "Правовое основание не должно быть длиннее 700 символов.")]
        [Display(Name = "Правовое основание")]
        public string LegalBasis { get; set; } = string.Empty;

        [Display(Name = "Требуется указать получателя документа")]
        public bool IsRecipientRequired { get; set; }

        [Display(Name = "Требуется указать период обучения")]
        public bool IsStudyPeriodRequired { get; set; }

        [Display(Name = "Требуется указать место работы")]
        public bool IsWorkplaceRequired { get; set; }

        [Display(Name = "Требуется оригинальная подпись")]
        public bool RequiresOriginalSignature { get; set; } = true;

        [Display(Name = "Доступен для выбора")]
        public bool IsActive { get; set; } = true;

        [Display(Name = "Порядок отображения")]
        public int SortOrder { get; set; }
    }
}