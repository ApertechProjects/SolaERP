namespace SolaERP.Application.Dtos.Attachment
{
    public class AttachmentWithFileDto
    {
        public int AttachmentId { get; set; }
        public string Name { get; set; }
        public byte[] FileData { get; set; }
        public string Base64 { get; set; }
        public int SourceId { get; set; }
        public int SourceTypeId { get; set; }
        public string Reference { get; set; }
        public string ExtensionType { get; set; }
        public int AttachmentTypeId { get; set; }
        public int AttachmentSubTypeId { get; set; }
        public DateTime UploadDateTime { get; set; }
        public int Size { get; set; }

    }
}
