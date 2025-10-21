using SolaERP.Application.Attributes;
using SolaERP.Application.Enums;
using System.Text.Json.Serialization;

namespace SolaERP.Application.Entities.Auth
{
    public class User : BaseEntity
    {
        [DbColumn("Id")]    
        public int UserId { get; set; }
        private string theme = "light";
        public int Id { get; set; } = 0;
        public string FullName { get; set; }
        public bool ChangePassword { get; set; }
        public int StatusId { get; set; }
        public string Theme
        {
            get
            {
                return theme;
            }
            set
            {
                theme = value;
            }
        }
        public DateTime LastActivity { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }

        [JsonIgnore]
        public string PasswordHash { get; set; }

        [DbIgnore]
        public string Password { get; set; }
        public string PhoneNumber { get; set; }
        public int UserTypeId { get; set; }
        public int VendorId { get; set; }
        public Guid UserToken { get; set; }
        public bool IsDeleted { get; set; }
        public int Gender { get; set; }
        public string Buyer { get; set; }
        public string Description { get; set; }
        public string ERPUser { get; set; }
        public string UserPhoto { get; set; }
        public string SignaturePhoto { get; set; }
        public bool InActive { get; set; }
        public string RefreshToken { get; set; }
        public DateTime RefreshTokenEndDate { get; set; }
        public string VerifyToken { get; set; }
        public Enums.Language Language { get; set; } = Enums.Language.en;
        public int? DefaultBusinessUnitId { get; set; }
        public string BusinessUnitCode { get; set; }
        public string HomePageReportFileId { get; set; }
    }
}
