namespace SolaERP.Application.Entities.Translate
{
    public class Translate : BaseEntity
    {
        public Int64 TranslateId { get; set; }
        public string LanguageCode { get; set; }
        public string Key { get; set; }
        public string Text { get; set; }
    }
}
