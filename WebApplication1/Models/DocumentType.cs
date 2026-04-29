using System.ComponentModel.DataAnnotations;

namespace EduDocFlow.Web.Models
{
    public enum DocumentTypeCategory
    {
        [Display(Name = "Справка")]
        Certificate = 1,

        [Display(Name = "Заявление")]
        Application = 2,

        [Display(Name = "Служебный документ")]
        InternalDocument = 3
    }

    public enum DocumentTemplateKind
    {
        [Display(Name = "Без шаблона")]
        None = 0,

        [Display(Name = "Справка с места учёбы")]
        StudyPlaceCertificate = 1,

        [Display(Name = "Справка об обучении или о периоде обучения")]
        StudyPeriodCertificate = 2,

        [Display(Name = "Справка о размере стипендии")]
        ScholarshipCertificate = 3,

        [Display(Name = "Справка о проживании в общежитии")]
        DormitoryCertificate = 4,

        [Display(Name = "Справка-вызов")]
        CallCertificate = 5,

        [Display(Name = "Характеристика с места учебы")]
        StudentCharacteristic = 6,

        [Display(Name = "Заявление на выдачу справки")]
        CertificateRequestApplication = 20,

        [Display(Name = "Заявление на академический отпуск")]
        AcademicLeaveApplication = 21,

        [Display(Name = "Заявление на перевод")]
        TransferApplication = 22,

        [Display(Name = "Заявление на отчисление")]
        ExpulsionApplication = 23,

        [Display(Name = "Заявление на восстановление")]
        ReinstatementApplication = 24,

        [Display(Name = "Заявление на выдачу дубликата документа")]
        DuplicateDocumentApplication = 25,

        [Display(Name = "Заявление на изменение персональных данных")]
        PersonalDataChangeApplication = 26
    }

    public class DocumentType
    {
        public int Id { get; set; }

        [Required]
        [StringLength(64)]
        [Display(Name = "Код документа")]
        public string Code { get; set; } = string.Empty;

        [Required]
        [StringLength(200)]
        [Display(Name = "Наименование документа")]
        public string Name { get; set; } = string.Empty;

        [Display(Name = "Категория")]
        public DocumentTypeCategory Category { get; set; }

        [Display(Name = "Вид шаблона")]
        public DocumentTemplateKind TemplateKind { get; set; }

        [StringLength(1000)]
        [Display(Name = "Описание")]
        public string Description { get; set; } = string.Empty;

        [StringLength(700)]
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
