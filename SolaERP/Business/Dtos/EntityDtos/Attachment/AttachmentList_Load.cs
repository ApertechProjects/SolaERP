using System.Text;

namespace SolaERP.Business.Dtos.EntityDtos.Attachment
{
    public partial class AttachmentList_Load
    {
        public System.Int32 AttachmentId { get; set; }
        public System.String FileName { get; set; }
        public System.Int32 SourceId { get; set; }
        public System.Int32 SourceTypeId { get; set; }
        public System.String Reference { get; set; }
        public System.String ExtensionType { get; set; }
        public System.Int32 AttachmentTypeId { get; set; }
        public System.Int32 AttachmentSubTypeId { get; set; }
        public System.DateTime UploadDateTime { get; set; }
        public System.Int32 Size { get; set; }
    }

    public partial class AttachmentList_Load
    {
        public string UploadDate
        {
            get => UploadDateTime.ToString("dd.MM.yyyy");
        }

        public string FileSizeAsMB
        {
            get
            {
                double mb = (Size / 1024f) / 1024f;
                var resSize = mb.ToString("0.00");
                return $"{resSize} MB";
            }
        }
        public byte[] FileData { get; set; }

    }
}
