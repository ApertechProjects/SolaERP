namespace SolaERP.Application.Entities.ApproveStage
{
    public class GroupApproveRole : BaseEntity
    {
        public int GroupApproveRoleId { get; set; }
        public int GroupId { get; set; }
        public int ApproveRoleId { get; set; }
    }
}
