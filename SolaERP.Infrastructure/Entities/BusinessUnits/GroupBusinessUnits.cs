namespace SolaERP.Application.Entities.BusinessUnits
{
    public class GroupBusinessUnits : BaseEntity
    {
        public int GroupBusinessUnitId { get; set; }
        public int GroupId { get; set; }
        public int BusinessUnitId { get; set; }
    }
}
