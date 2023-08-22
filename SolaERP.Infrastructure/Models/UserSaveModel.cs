using Microsoft.AspNetCore.Http;
using System.Text.Json.Serialization;

namespace SolaERP.Application.Models
{
    public class UserSaveModel
    {
        public int Id { get; set; } = 0;
        public string FullName { get; set; }
        public bool ChangePassword { get; set; }
        public int StatusId { get; set; }
        public string Theme { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public string PhoneNumber { get; set; }
        public int UserTypeId { get; set; }
        public int VendorId { get; set; }
        public int Gender { get; set; }
        public string Buyer { get; set; }
        public string Description { get; set; }
        public string ERPUser { get; set; }
        public bool IsDeleted { get; set; }
        public IFormFile SignaturePhoto { get; set; }
        public bool SignaturePhotoIsDeleted { get; set; }
        public IFormFile UserPhoto { get; set; }
        public bool UserPhotoIsDeleted { get; set; }
        public bool InActive { get; set; }
        public int? DefaultBusinessUnitId { get; set; }


    }
}
