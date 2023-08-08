namespace SolaERP.Application.Entities.Groups
{
    public class AddUserToGroupModel
    {
        public int UserId { get; set; }
        public List<int> groupIds { get; set; }
    }
}
