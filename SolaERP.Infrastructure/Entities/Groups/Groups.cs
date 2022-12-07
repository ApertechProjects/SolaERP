namespace SolaERP.Infrastructure.Entities.Groups
{
    public class Groups : BaseEntity
    {
        public int GroupId { get; set; }
        public string GroupName { get; set; }
        public string Description { get; set; }
    }
}
