namespace SolaERP.Infrastructure.Entities.BusinessUnits
{
    public class BusinessUnitForGroup : BaseBusinessUnit
    {
        public int GroupBusinessUnitId { get; set; }
        public bool IsInGroup { get; set; }
    }
}
