namespace SolaERP.Infrastructure.Entities.BusinessUnits
{
    public class BusinessUnits : BaseEntity
    {
        public int BusinessUnitId { get; set; }
        public string BusinessUnitCode { get; set; }
        public string BusinessUnitName { get; set; }
        public string TaxId { get; set; }
        public string Address { get; set; }
        public string CountryCode { get; set; }
        public string FullName { get; set; }
        public string Position { get; set; }

    }
}
