namespace SolaERP.Application.Models
{
    public class AttachmentSaveModel
    {
        public int AttachmentId { get; set; }
        public string Name { get; set; }
        public string Filebase64 { get; set; }
        public int SourceId { get; set; }
        public string SourceType { get; set; }
        public string ExtensionType { get; set; }
        public int AttachmentTypeId { get; set; }
        public int AttachmentSubTypeId { get; set; }
        public int Size { get; set; }
        public int Type { get; set; }
    }
}
