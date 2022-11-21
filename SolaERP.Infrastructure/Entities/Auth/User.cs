
namespace SolaERP.Infrastructure.Entities.Auth
{
    public class User : BaseEntity
    {
        string _name;
        string _email;
        public User()
        {
            this.CreatedOn = DateTime.Now;
            this.LastActivity = DateTime.Now;
            this.ExpirationDate = new DateTime(2099, 12, 31);
            this.LockoutEnd = DateTime.Now;
            this.RowIndex = -1;
            this.NotificationEmail = "NULL";
            this.Theme = "light";
            this.ReturnMessage = "NULL";
            this.CreatedOn = DateTime.Now;
            this.UpdatedOn = DateTime.Now;
            this.SecurityStamp = "NULL";
            this.ConcurrencyStamp = "NULL";
            this.PhoneNumber = "NULL";
            this.SyteLineUserCode = "NULL";
            this.Position = "NULL";
        }

        public int Id { get; set; }
        public int RowIndex { get; set; }
        public string FullName { get; set; }
        public string NotificationEmail { get; set; }
        public bool ChangePassword { get; set; } = false;
        public int StatusId { get; set; }
        public string Theme { get; set; }
        public DateTime ExpirationDate { get; set; }
        public int Sessions { get; set; }
        public DateTime LastActivity { get; set; }
        public byte Photo { get; set; }
        public string ReturnMessage { get; set; }
        public DateTime CreatedOn { get; set; }
        public int CreatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
        public int UpdatedBy { get; set; }
        public string UserName
        {
            get { return _name; }
            set { _name = value; }
        }
        public string NormalizedUserName
        {
            get { return _name.ToUpper(); }
            set { _name = value; }
        }
        public string Email
        {
            get { return _email; }
            set { _email = value; }

        }
        public string NormalizedEmail
        {
            get { return _email.ToUpper(); }
            set { _email = value; }
        }
        public bool EmailConfirmed { get; set; }
        public string PasswordHash { get; set; }
        public string SecurityStamp { get; set; }
        public string ConcurrencyStamp { get; set; }
        public string PhoneNumber { get; set; }
        public bool PhoneNumberConfirmed { get; set; } = true;
        public bool TwoFactorEnabled { get; set; } = false;
        public DateTime LockoutEnd { get; set; }
        public bool LockoutEnabled { get; set; }
        public int AccessFailedCount { get; set; } = 0;
        public string SyteLineUserCode { get; set; } 
        public int UserTypeId { get; set; }
        public int CompanyId { get; set; }
        public string Position { get; set; } 
        public int VendorId { get; set; }
        public Guid UserToken { get; set; }
    }
}
