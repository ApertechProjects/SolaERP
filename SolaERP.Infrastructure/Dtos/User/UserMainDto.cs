namespace SolaERP.Infrastructure.Dtos.User
{
    public class UserMainDto
    {
        public int Id { get; set; }
        public int StatusId { get; set; }
        public string Email { get; set; }
        public string Photo { get; set; }
        public string FullName { get; set; }
        public string ApproveStatus { get; set; }
        public Int64 RowNum { get; set; }
        public string Status { get; set; }
        public DateTime LastActivity { get; set; }
        public string UserName { get; set; }
        public string Description { get; set; }
        public int Gender { get; set; }
        public string Buyer { get; set; }
        public string VendorCode { get; set; }
        public string VendorName { get; set; }
        public string TaxId { get; set; }
        public string PhoneNumber { get; set; }
        public int UserTypeId { get; set; }
        public string ERPUser { get; set; }
        public string Theme { get; set; }
        public int VendorId { get; set; }
        public int Sequence { get; set; }
    }
}
