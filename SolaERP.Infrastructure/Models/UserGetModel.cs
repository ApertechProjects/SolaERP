namespace SolaERP.Infrastructure.Models
{
    public class UserGetModel
    {
        public List<int> UserStatus { get; set; }
        public List<int> UserType { get; set; }
    }


    public class UserWFAGetRequest
    {
        public int UserStatus { get; set; }
        public int UserType { get; set; }
    }


    public class UserAllQueryRequest
    {
        public int UserStatus { get; set; }
        public int UserType { get; set; }
    }
}
