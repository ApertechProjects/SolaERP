namespace SolaERP.Application.Entities.Language
{
    public class Language : BaseEntity
    {
        public Int64 LanguageId { get; set; }
        public string LanguageCode { get; set; }
        public string LanguageName { get; set; }
        public int IsActive { get; set; }
    }
}
