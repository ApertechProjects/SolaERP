
namespace SolaERP.Infrastructure.Dtos.User
{
    public class UserLoadDto
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public bool ChangePassword { get; set; }
        public int StatusId { get; set; }
        public string Theme { get; set; }
        public DateTime LastActivity { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public int UserTypeId { get; set; }
        public int VendorId { get; set; }
        public bool IsDeleted { get; set; }
        public int Gender { get; set; }
        public string Buyer { get; set; }
        public string UserPhoto { get; set; }
        public string SignaturePhoto { get; set; }
        public string Description { get; set; }
        public string ERPUser { get; set; }
        public int Session { get; set; }
    }
}
