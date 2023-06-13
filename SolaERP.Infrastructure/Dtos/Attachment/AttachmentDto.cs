namespace SolaERP.Application.Dtos.Attachment
{
    public class AttachmentDto
    {
        public int AttachmentId { get; set; }
        public string Name { get; set; }
        public int SourceId { get; set; }
        public int SourceTypeId { get; set; }
        public string Reference { get; set; }
        public string ExtensionType { get; set; }
        public int AttachmentTypeId { get; set; }
        public int AttachmentSubTypeId { get; set; }
        public DateTime LastModifiedDate { get; set; }
        public int Size { get; set; }
        public string FileLink { get; set; }
    }
}
