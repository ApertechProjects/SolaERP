namespace SolaERP.Application.Dtos.Attachment
{
    public class AttachmentWithFileDto
    {
        public int AttachmentId { get; set; }
        public string Name { get; set; }
        public string FileData { get; set; }
        public string Preview { get; set; }
        public int SourceId { get; set; }
        public int SourceTypeId { get; set; }
        public string Reference { get; set; }
        public string ExtensionType { get; set; }
        public int AttachmentTypeId { get; set; }
        public int AttachmentSubTypeId { get; set; }
        public DateTime LastModifiedDate { get; set; }
        public int Size { get; set; }

    }
}
