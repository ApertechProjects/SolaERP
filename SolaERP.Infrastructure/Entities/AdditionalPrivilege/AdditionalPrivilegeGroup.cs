namespace SolaERP.Infrastructure.Entities.AdditionalPrivilege
{
    public class AdditionalPrivilegeGroup : BaseEntity
    {
        public int GroupAdditionalPrivilegeId { get; set; }
        public int GroupId { get; set; }
        public int VendorDraft { get; set; }
    }
}
