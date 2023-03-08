using System.Text;

namespace SolaERP.Business.Dtos.GeneralDtos
{
    public class AppAttachment
    {
        public string FileName { get; set; } = " ";
        public string FileBase64 { get; set; } = " ";
        public byte[] FileData { get { return Encoding.ASCII.GetBytes(FileBase64); } }
        public int SourceId { get; set; }
        public string SourceType { get; set; }
        public string Reference { get; set; }
        public string ExtensionType { get; set; }
        public int AttachmentTypeId { get; set; }
        public int AttachmentSubTypeId { get; set; }
        public DateTime UploadDateTime { get { return DateTime.Now; } }
        public int Size { get; set; }
    }
}
