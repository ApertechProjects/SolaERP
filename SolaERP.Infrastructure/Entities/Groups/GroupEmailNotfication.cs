namespace SolaERP.Infrastructure.Entities.Groups
{
    public class GroupEmailNotfication : BaseEntity
    {
        public int GroupEmailNotficationId { get; set; }
        public int GroupId { get; set; }
        public int EmailNotficationId { get; set; }
    }
}
