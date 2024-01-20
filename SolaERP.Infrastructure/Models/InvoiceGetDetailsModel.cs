namespace SolaERP.Application.Models
{
    public class InvoiceGetDetailsModel
    {
        public int BusinessUnitId { get; set; }
        public int OrderMainId { get; set; }
        public DateTime? Date { get; set; }
        public List<string> GRNs { get; set; }
        public decimal AdvanceAmount { get; set; }
    }
}
