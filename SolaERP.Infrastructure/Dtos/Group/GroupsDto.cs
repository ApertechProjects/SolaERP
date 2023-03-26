namespace SolaERP.Infrastructure.Dtos.Group
{
    public class GroupsDto
    {
        public int GroupId { get; set; }
        public string GroupName { get; set; }
        public string Description { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
