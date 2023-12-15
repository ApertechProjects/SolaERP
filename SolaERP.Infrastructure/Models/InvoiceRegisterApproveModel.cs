namespace SolaERP.Application.Models
{
    public class InvoiceRegisterApproveModel
    {
        public List<InvoiceRegisterIdModel> InvoiceRegisterIds { get; set; }
        public int ApproveStatus { get; set; }
        public string Comment { get; set; }
        public int BusinessUnitId { get; set; }
        public int? RejectReasonId { get; set; }

    }

    public class InvoiceRegisterIdModel
    {
        public int InvoiceRegisterId { get; set; }
        public int Sequence { get; set; }
        public bool InMaxSequence { get; set; }
        public int InvoiceTypeId { get; set; }
    }
}
