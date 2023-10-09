namespace SolaERP.Application.Dtos.Payment
{
    public class AttachmentDto
    {
        public int AttachmentTypeId { get; set; }
        public string AttachmentType { get; set; }
        public int AttachmentSubTypeId { get; set; }
        public string AttachmentSubType { get; set; }
        public bool Checked { get; set; }
    }
}
