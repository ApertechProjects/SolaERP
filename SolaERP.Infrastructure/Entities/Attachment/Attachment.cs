using SolaERP.Infrastructure.Attributes;

namespace SolaERP.Infrastructure.Entities.Attachment
{
    public class Attachment : BaseEntity
    {
        public int AttachmentId { get; set; }
        public string FileName { get; set; }
        public byte[] FileData { get; set; }
        public int SourceId { get; set; }
        public int SourceTypeId { get; set; }
        public string Reference { get; set; }
        public string ExtensionType { get; set; }
        public int AttachmentTypeId { get; set; }
        public int AttachmentSubTypeId { get; set; }
        public DateTime UploadDateTime { get; set; }
        public int Size { get; set; }


        public static void SetIgnoredProperty(params string[] properties)
        {
            if (properties != null && properties.Length > 0)
            {
                foreach (var property in properties)
                {
                    var propertyInfo = typeof(Attachment).GetProperty(property);
                    var props = typeof(Attachment).GetProperties();

                    if (propertyInfo != null)
                    {
                        DbIgnoreAttribute ignoreAttribute = new();
                        //propertyInfo.CustomAttributes.ToList().Add(ignoreAttribute);
                    }
                }
            }
        }
    }
}
