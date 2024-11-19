namespace SolaERP.Application.Entities.BusinessUnits
{
    public class BaseBusinessUnit : BaseEntity
    {
        public int BusinessUnitId { get; set; }
        public string BusinessUnitCode { get; set; }
        public string BusinessUnitName { get; set; }
        public string BankChargeAccount { get; set; }
        public string ConnectionData { get; set; }
		public int UseOrderForInvoice { get; set; }
	}
}
