using System.Text;

namespace SolaERP.Infrastructure.Dtos.Attachment
{
    public class AttachmentWithFileDto
    {
        public int AttachmentId { get; set; }
        public string FileName { get; set; }
        public string Filebase64 { get; set; }
        public byte[] FileData { get => Encoding.UTF8.GetBytes(Filebase64); }
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
