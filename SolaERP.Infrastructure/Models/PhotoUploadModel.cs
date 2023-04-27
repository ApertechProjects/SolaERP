using SolaERP.Application.Enums;

namespace SolaERP.Application.Models
{
    public class PhotoUploadModel
    {
        public string? Base64Photo { get; set; }
        public string? Base64Signature { get; set; }
        public string? PhotoFileName { get; set; }
        public FileExtension Extension { get; set; }
    }
}
