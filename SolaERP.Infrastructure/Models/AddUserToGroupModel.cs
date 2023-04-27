namespace SolaERP.Infrastructure.Models
{
    public class AddUserToGroupModel
    {
        public int UserId { get; set; }
        public List<int> groupIds { get; set; }
    }
}
