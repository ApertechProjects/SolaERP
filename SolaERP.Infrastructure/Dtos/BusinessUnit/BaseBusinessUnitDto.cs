namespace SolaERP.Application.Dtos.BusinessUnit
{
    public class BaseBusinessUnitDto
    {
        public int BusinessUnitId { get; set; }
        public string BusinessUnitCode { get; set; }
        public string BusinessUnitName { get; set; }
        public string BankChargeAccount { get; set; }
		    public bool UseOrderForInvoice { get; set; }
        public decimal? VATAccount { get; set; }
        public bool HasVATAccount { get; set; }
        public decimal ExportOilPercent { get; set; }
    }
}
