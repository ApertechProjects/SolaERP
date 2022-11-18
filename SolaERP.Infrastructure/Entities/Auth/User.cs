namespace SolaERP.Infrastructure.Entities.Auth
{
    public class User
    {
        public System.Int32 Id { get; set; }
        public System.Int32 RowIndex { get; set; }
        public System.String FullName { get; set; }
        public System.String NotificationEmail { get; set; }
        public System.Boolean ChangePassword { get; set; }
        public System.Int32 StatusId { get; set; }
        public System.String Theme { get; set; }
        public System.DateTime ExpirationDate { get; set; }
        public System.Int32 Sessions { get; set; }
        public System.DateTime LastActivity { get; set; }
        public System.Byte[] Photo { get; set; }
        public System.String ReturnMessage { get; set; }
        public System.DateTime CreatedOn { get; set; }
        public System.Int32 CreatedBy { get; set; }
        public System.DateTime UpdatedOn { get; set; }
        public System.Int32 UpdatedBy { get; set; }
        public System.String UserName { get; set; }
        public System.String NormalizedUserName { get; set; }
        public System.String Email { get; set; }
        public System.String NormalizedEmail { get; set; }
        public System.Boolean EmailConfirmed { get; set; }
        public System.String PasswordHash { get; set; }
        public System.String SecurityStamp { get; set; }
        public System.String ConcurrencyStamp { get; set; }
        public System.String PhoneNumber { get; set; }
        public System.Boolean PhoneNumberConfirmed { get; set; }
        public System.Boolean TwoFactorEnabled { get; set; }
        public System.DateTimeOffset LockoutEnd { get; set; }
        public System.Boolean LockoutEnabled { get; set; }
        public System.Int32 AccessFailedCount { get; set; }
        public System.String SyteLineUserCode { get; set; }
        public System.Int32 UserTypeId { get; set; }
        public System.Int32 CompanyId { get; set; }
        public System.String Position { get; set; }
        public System.Int32 VendorId { get; set; }
        public System.Guid UserToken { get; set; }
    }
}
