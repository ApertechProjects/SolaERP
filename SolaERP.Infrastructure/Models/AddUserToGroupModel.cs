namespace SolaERP.Application.Models
{
    public class AddUserToGroupModel
    {
        public int UserId { get; set; }
        public List<int> groupIds { get; set; }
    }
}
