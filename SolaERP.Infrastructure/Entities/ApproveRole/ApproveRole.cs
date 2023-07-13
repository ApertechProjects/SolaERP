namespace SolaERP.Application.Entities.ApproveRole
{
    public class ApproveRole : BaseEntity
    {
        public int ApproveRoleId { get; set; }
        public string ApproveRoleName { get; set; }
        public string CreatedUser { get; set; }
        public DateTime CreatedDate { get; set; }
        public int BusinessUnitId { get; set; }

    }
}
