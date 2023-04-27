namespace SolaERP.Application.Entities.Groups
{
    public class Group : BaseEntity
    {
        public int GroupId { get; set; }
        public string GroupName { get; set; }
        public string Description { get; set; }
    }
}
