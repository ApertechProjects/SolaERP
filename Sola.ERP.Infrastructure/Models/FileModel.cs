using SolaERP.Infrastructure.Enums;

namespace SolaERP.Infrastructure.Models
{
    public class FileModel
    {
        public string Filename { get; set; }
        public byte[] Data { get; set; }
        public Enums.FileExtensions Extension { get; set; }



        public string GetPath()
        {
            return Extension switch
            {
                FileExtensions.PNG => @"\sources\images",
            };
        }
    }
}
