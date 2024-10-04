using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolaERP.Application.Entities.User
{
    public class UserEntity : BaseEntity
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public bool ChangePassword { get; set; }
        public int StatusId { get; set; }
        public string Theme { get; set; }
        public DateTime? LastActivity { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string PhoneNumber { get; set; }
        public int UserTypeId { get; set; }
        public int? VendorId { get; set; }
        public Guid UserToken { get; set; }
        public bool? IsDeleted { get; set; }
        public int Gender { get; set; }
        public string Buyer { get; set; }
        public string Description { get; set; }
        public string ERPUser { get; set; }
        public string UserPhoto { get; set; }
        public string SignaturePhoto { get; set; }
        public string EmailVerificationToken { get; set; }
        public bool InActive { get; set; }
        public int? Session { get; set; }
        public string RefreshToken { get; set; }
        public DateTime? RefreshTokenEndDate { get; set; }
        public bool EmailVerified { get; set; }
        public string VerifyToken { get; set; }
        public string Language { get; set; }
        public int? DefaultBusinessUnitId { get; set; }
        public string HomePageReportFileId { get; set; }
    }
}
