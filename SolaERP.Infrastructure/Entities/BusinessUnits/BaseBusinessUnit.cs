namespace SolaERP.Infrastructure.Entities.BusinessUnits
{
    public class BaseBusinessUnit : BaseEntity
    {
        public int BusinessUnitId { get; set; }
        public string BusinessUnitCode { get; set; }
        public string BusinessUnitName { get; set; }
    }
}
