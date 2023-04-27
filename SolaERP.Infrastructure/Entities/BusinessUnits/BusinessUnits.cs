namespace SolaERP.Application.Entities.BusinessUnits
{
    public class BusinessUnits : BaseBusinessUnit
    {
        public string TaxId { get; set; }
        public string Address { get; set; }
        public string CountryCode { get; set; }
        public string FullName { get; set; }
        public string Position { get; set; }
    }
}
