namespace SolaERP.Application.Models
{
    public class AttachmentSaveModel
    {
        private int _attachmentId;
        public int AttachmentId
        {
            get
            {
                if (_attachmentId < 0)
                    _attachmentId = 0;
                return _attachmentId;
            }
            set
            {
                _attachmentId = value;
            }
        }
        public string Name { get; set; }
        public int SourceId { get; set; }
        public string SourceType { get; set; }
        public string Reference { get; set; }
        public string ExtensionType { get; set; }
        public int AttachmentTypeId { get; set; }
        public int AttachmentSubTypeId { get; set; }
        public DateTime LastModifiedDate { get; set; }
        public int Size { get; set; }
        public string FileLink { get; set; }
        public int Type { get; set; }
    }
}
