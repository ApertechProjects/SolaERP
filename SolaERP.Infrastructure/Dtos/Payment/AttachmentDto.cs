namespace SolaERP.Application.Dtos.Payment
{
    public class AttachmentDto
    {
        public int PaymentDocumentTypeId { get; set; }
        public string PaymentDocumentType { get; set; }
        public int PaymentDocumentSubTypeId { get; set; }
        public string PaymentDocumentSubType { get; set; }
        public bool Checked { get; set; }
    }
}
