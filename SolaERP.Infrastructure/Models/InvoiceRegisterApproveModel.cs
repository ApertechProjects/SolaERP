namespace SolaERP.Application.Models
{
    public class InvoiceRegisterApproveModel
    {
        public List<InvoiceRegisterIdModel> InvoiceRegisterIds { get; set; }
        public int ApproveStatus { get; set; }
        public string Comment { get; set; }
    }

    public class InvoiceRegisterIdModel
    {
        public int InvoiceRegisterId { get; set; }
        public int Sequence { get; set; }
    }
}
