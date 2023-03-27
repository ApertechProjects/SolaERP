namespace SolaERP.Infrastructure.Models
{
    public class UserGetModel
    {
        public bool AllUserStatus { get; set; }
        public bool AllUserTypes { get; set; }
        public List<int> UserStatus { get; set; }
        public List<int> UserType { get; set; }
    }
}
