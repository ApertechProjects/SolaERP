namespace SolaERP.Infrastructure.Models
{
    public class AttachmentSaveModel
    {
        public int AttachmentId { get; set; }
        public string FileName { get; set; }
        public byte[] FileData { get; set; }
        public int SourceId { get; set; }
        public string SourceType { get; set; }
        public string Reference { get; set; }
        public string ExtensionType { get; set; }
        public int AttachmentTypeId { get; set; }
        public int AttachmentSubTypeId { get; set; }
        public int Size { get; set; }
    }
}
