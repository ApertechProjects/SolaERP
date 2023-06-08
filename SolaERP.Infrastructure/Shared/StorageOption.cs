namespace SolaERP.Application.Shared
{
    public class StorageOption
    {
        public string IP { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string BaseFolderPath { get; set; }

        public string GetImageFolderPath()
            => Path.Combine(BaseFolderPath, @"\profiles");

        public string GetSignatureFolderPath()
            => Path.Combine(BaseFolderPath, @"\signatures");

        public string GetAttachmentsFolderPath()
            => Path.Combine(BaseFolderPath, @"\attachments");
    }
}
