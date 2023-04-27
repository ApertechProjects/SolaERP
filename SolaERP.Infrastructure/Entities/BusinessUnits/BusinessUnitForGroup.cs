namespace SolaERP.Application.Entities.BusinessUnits
{
    public class BusinessUnitForGroup : BaseBusinessUnit
    {
        public int GroupBusinessUnitId { get; set; }
        public int BusinessUnitId { get; set; }
        public string BusinessUnitCode { get; set; }
        public string BusinessUnitName { get; set; }
        public bool IsInGroup { get; set; }
    }
}
