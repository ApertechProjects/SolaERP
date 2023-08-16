using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolaERP.Application.Dtos.Attachment
{
    public class AttachmentSaveDto
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
        public string Reference { get; set; }
        public string ExtensionType { get; set; }
        public int AttachmentTypeId { get; set; }
        public int AttachmentSubTypeId { get; set; }
        public DateTime LastModifiedDate { get; set; }
        public int Size { get; set; }
        public IFormFile File { get; set; }
        public int Type { get; set; }
    }
}
