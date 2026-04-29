using EduDocFlow.Web.Models;
using Microsoft.EntityFrameworkCore;

namespace EduDocFlow.Web.Data
{
    public static class DbInitializer
    {
        public static async Task InitializeAsync(ApplicationDbContext context)
        {
            await context.Database.MigrateAsync();

            if (!await context.DocumentTypes.AnyAsync())
            {
                var documentTypes = new List<DocumentType>
                {
                    new DocumentType
                    {
                        Code = "STUDY_PLACE_CERTIFICATE",
                        Name = "Справка с места учёбы",
                        Category = DocumentTypeCategory.Certificate,
                        TemplateKind = DocumentTemplateKind.StudyPlaceCertificate,
                        Description = "Подтверждает факт обучения студента в образовательной организации на текущий момент.",
                        LegalBasis = "Выдаётся на основании сведений, содержащихся в информационной системе образовательной организации.",
                        IsRecipientRequired = true,
                        IsStudyPeriodRequired = false,
                        IsWorkplaceRequired = false,
                        RequiresOriginalSignature = true,
                        IsActive = true,
                        SortOrder = 10
                    },

                    new DocumentType
                    {
                        Code = "STUDY_PERIOD_CERTIFICATE",
                        Name = "Справка об обучении или о периоде обучения",
                        Category = DocumentTypeCategory.Certificate,
                        TemplateKind = DocumentTemplateKind.StudyPeriodCertificate,
                        Description = "Содержит сведения об обучении студента или о периоде его обучения в образовательной организации.",
                        LegalBasis = "Формируется на основании данных о зачислении, обучении, переводе или отчислении студента.",
                        IsRecipientRequired = true,
                        IsStudyPeriodRequired = true,
                        IsWorkplaceRequired = false,
                        RequiresOriginalSignature = true,
                        IsActive = true,
                        SortOrder = 20
                    },

                    new DocumentType
                    {
                        Code = "SCHOLARSHIP_CERTIFICATE",
                        Name = "Справка о размере стипендии",
                        Category = DocumentTypeCategory.Certificate,
                        TemplateKind = DocumentTemplateKind.ScholarshipCertificate,
                        Description = "Подтверждает назначение и размер стипендии студента.",
                        LegalBasis = "Формируется на основании данных бухгалтерии или стипендиальной комиссии образовательной организации.",
                        IsRecipientRequired = true,
                        IsStudyPeriodRequired = true,
                        IsWorkplaceRequired = false,
                        RequiresOriginalSignature = true,
                        IsActive = true,
                        SortOrder = 30
                    },

                    new DocumentType
                    {
                        Code = "DORMITORY_CERTIFICATE",
                        Name = "Справка о проживании в общежитии",
                        Category = DocumentTypeCategory.Certificate,
                        TemplateKind = DocumentTemplateKind.DormitoryCertificate,
                        Description = "Подтверждает проживание студента в общежитии образовательной организации.",
                        LegalBasis = "Формируется на основании сведений о заселении студента в общежитие.",
                        IsRecipientRequired = true,
                        IsStudyPeriodRequired = false,
                        IsWorkplaceRequired = false,
                        RequiresOriginalSignature = true,
                        IsActive = true,
                        SortOrder = 40
                    },

                    new DocumentType
                    {
                        Code = "CALL_CERTIFICATE",
                        Name = "Справка-вызов",
                        Category = DocumentTypeCategory.Certificate,
                        TemplateKind = DocumentTemplateKind.CallCertificate,
                        Description = "Используется для подтверждения необходимости участия студента в учебных мероприятиях, сессии или промежуточной аттестации.",
                        LegalBasis = "Оформляется на основании учебного графика и сведений о форме обучения студента.",
                        IsRecipientRequired = true,
                        IsStudyPeriodRequired = true,
                        IsWorkplaceRequired = true,
                        RequiresOriginalSignature = true,
                        IsActive = true,
                        SortOrder = 50
                    },

                    new DocumentType
                    {
                        Code = "STUDENT_CHARACTERISTIC",
                        Name = "Характеристика с места учебы",
                        Category = DocumentTypeCategory.Certificate,
                        TemplateKind = DocumentTemplateKind.StudentCharacteristic,
                        Description = "Содержит краткую характеристику студента, сведения об обучении, дисциплине и участии в жизни образовательной организации.",
                        LegalBasis = "Подготавливается на основании сведений куратора, учебной части и иных подразделений образовательной организации.",
                        IsRecipientRequired = true,
                        IsStudyPeriodRequired = false,
                        IsWorkplaceRequired = false,
                        RequiresOriginalSignature = true,
                        IsActive = true,
                        SortOrder = 60
                    },

                    new DocumentType
                    {
                        Code = "CERTIFICATE_REQUEST_APPLICATION",
                        Name = "Заявление на выдачу справки",
                        Category = DocumentTypeCategory.Application,
                        TemplateKind = DocumentTemplateKind.CertificateRequestApplication,
                        Description = "Заявление студента на получение справки из образовательной организации.",
                        LegalBasis = "Рассматривается учебной частью в установленном внутренними правилами порядке.",
                        IsRecipientRequired = true,
                        IsStudyPeriodRequired = false,
                        IsWorkplaceRequired = false,
                        RequiresOriginalSignature = false,
                        IsActive = true,
                        SortOrder = 100
                    },

                    new DocumentType
                    {
                        Code = "ACADEMIC_LEAVE_APPLICATION",
                        Name = "Заявление на академический отпуск",
                        Category = DocumentTypeCategory.Application,
                        TemplateKind = DocumentTemplateKind.AcademicLeaveApplication,
                        Description = "Заявление студента о предоставлении академического отпуска.",
                        LegalBasis = "Рассматривается образовательной организацией с учётом документов, подтверждающих основание для академического отпуска.",
                        IsRecipientRequired = false,
                        IsStudyPeriodRequired = true,
                        IsWorkplaceRequired = false,
                        RequiresOriginalSignature = true,
                        IsActive = true,
                        SortOrder = 110
                    },

                    new DocumentType
                    {
                        Code = "TRANSFER_APPLICATION",
                        Name = "Заявление на перевод",
                        Category = DocumentTypeCategory.Application,
                        TemplateKind = DocumentTemplateKind.TransferApplication,
                        Description = "Заявление студента о переводе на другую образовательную программу, форму обучения или в другую группу.",
                        LegalBasis = "Рассматривается образовательной организацией с учётом учебного плана, наличия мест и локальных нормативных актов.",
                        IsRecipientRequired = false,
                        IsStudyPeriodRequired = false,
                        IsWorkplaceRequired = false,
                        RequiresOriginalSignature = true,
                        IsActive = true,
                        SortOrder = 120
                    },

                    new DocumentType
                    {
                        Code = "EXPULSION_APPLICATION",
                        Name = "Заявление на отчисление",
                        Category = DocumentTypeCategory.Application,
                        TemplateKind = DocumentTemplateKind.ExpulsionApplication,
                        Description = "Заявление студента об отчислении из образовательной организации по собственному желанию.",
                        LegalBasis = "Рассматривается учебной частью и используется как основание для подготовки приказа.",
                        IsRecipientRequired = false,
                        IsStudyPeriodRequired = false,
                        IsWorkplaceRequired = false,
                        RequiresOriginalSignature = true,
                        IsActive = true,
                        SortOrder = 130
                    },

                    new DocumentType
                    {
                        Code = "REINSTATEMENT_APPLICATION",
                        Name = "Заявление на восстановление",
                        Category = DocumentTypeCategory.Application,
                        TemplateKind = DocumentTemplateKind.ReinstatementApplication,
                        Description = "Заявление гражданина о восстановлении в число студентов образовательной организации.",
                        LegalBasis = "Рассматривается с учётом срока отчисления, академической разницы и локальных правил образовательной организации.",
                        IsRecipientRequired = false,
                        IsStudyPeriodRequired = false,
                        IsWorkplaceRequired = false,
                        RequiresOriginalSignature = true,
                        IsActive = true,
                        SortOrder = 140
                    },

                    new DocumentType
                    {
                        Code = "DUPLICATE_DOCUMENT_APPLICATION",
                        Name = "Заявление на выдачу дубликата документа",
                        Category = DocumentTypeCategory.Application,
                        TemplateKind = DocumentTemplateKind.DuplicateDocumentApplication,
                        Description = "Заявление на выдачу дубликата студенческого билета, зачётной книжки или иного документа.",
                        LegalBasis = "Рассматривается при утрате, порче или необходимости повторной выдачи документа.",
                        IsRecipientRequired = false,
                        IsStudyPeriodRequired = false,
                        IsWorkplaceRequired = false,
                        RequiresOriginalSignature = true,
                        IsActive = true,
                        SortOrder = 150
                    },

                    new DocumentType
                    {
                        Code = "PERSONAL_DATA_CHANGE_APPLICATION",
                        Name = "Заявление на изменение персональных данных",
                        Category = DocumentTypeCategory.Application,
                        TemplateKind = DocumentTemplateKind.PersonalDataChangeApplication,
                        Description = "Заявление студента об изменении фамилии, имени, отчества, паспортных данных или иных персональных сведений.",
                        LegalBasis = "Рассматривается на основании документов, подтверждающих изменение персональных данных.",
                        IsRecipientRequired = false,
                        IsStudyPeriodRequired = false,
                        IsWorkplaceRequired = false,
                        RequiresOriginalSignature = true,
                        IsActive = true,
                        SortOrder = 160
                    }
                };

                await context.DocumentTypes.AddRangeAsync(documentTypes);
                await context.SaveChangesAsync();
            }
        }
    }
}