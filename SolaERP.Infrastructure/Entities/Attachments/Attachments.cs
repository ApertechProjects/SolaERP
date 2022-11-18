using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolaERP.Infrastructure.Entities.Attachments
{
    public class Attachments : BaseEntity
    {
        public int AttachmentId { get; set; }
        public string FileName { get; set; }
        public byte FileData { get; set; }
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
