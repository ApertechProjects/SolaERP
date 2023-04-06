using SolaERP.Infrastructure.Enums;

namespace SolaERP.Infrastructure.Models
{
    public class PhotoUploadModel
    {
        public string base64Photo { get; set; }
        public string FileName { get; set; }
        public FileExtension Extension { get; set; }
    }
}
