using Microsoft.AspNetCore.Http;
using SolaERP.Application.Dtos.Attachment;
using SolaERP.Application.Entities;
using SolaERP.Application.Entities.SupplierEvaluation;
using SolaERP.Application.Entities.Vendors;
using SolaERP.Application.Models;

namespace SolaERP.Application.Dtos.SupplierEvaluation
{
    public class CompanyInformation
    {
        public CompanyInfoDto CompanyInfo { get; set; }
        public List<BusinessCategory> BusinessCategories { get; set; }
        public List<PrequalificationCategory> PrequalificationTypes { get; set; }
        public List<PaymentTerms> PaymentTerms { get; set; }
        public List<Country> Countries { get; set; }
        public List<ProductService> Services { get; set; }
    }

    public class CompanyInfoDto
    {
        public int? VendorId { get; set; }
        public int? ReviseNo { get; set; }
        public string ContactPerson { get; set; }
        public string Email { get; set; }
        public string PhoneNo { get; set; }
        public string VendorCode { get; set; }
        public string CompanyName { get; set; }
        public string TaxId { get; set; }
        public string TaxOffice { get; set; }
        public string CompanyAdress { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string WebSite { get; set; }
        public DateTime? RegistrationDate { get; set; }
        public string[] RepresentedCompanies { get; set; }
        public string[] RepresentedProducts { get; set; }
        public int CreditDays { get; set; }
        public string PaymentTerms { get; set; }
        public bool AgreeWithDefaultDays { get; set; }
        public string Other { get; set; }
        public List<ProductService> Services { get; set; }
        public List<PrequalificationCategory> PrequalificationTypes { get; set; }
        public List<BusinessCategory> BusinessCategories { get; set; }
        public List<BusinessSector> BusinessSectors { get; set; }
        public List<AttachmentSaveModel> CompanyLogo { get; set; }
        public IFormFile CompanyLogoFile { get; set; }
        public bool CompanyLogoFileIsDeleted { get; set; }
        public List<AttachmentSaveModel> Attachments { get; set; }
    }

    public class CompanyInfoViewDto
    {
        public int? VendorId { get; set; }
        public int? ReviseNo { get; set; }
        public string ContactPerson { get; set; }
        public string Email { get; set; }
        public string PhoneNo { get; set; }
        public string VendorCode { get; set; }
        public string CompanyName { get; set; }
        public string TaxId { get; set; }
        public string TaxOffice { get; set; }
        public string CompanyAdress { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string WebSite { get; set; }
        public DateTime? RegistrationDate { get; set; }
        public string[] RepresentedCompanies { get; set; }
        public string[] RepresentedProducts { get; set; }
        public int CreditDays { get; set; }
        public string PaymentTerms { get; set; }
        public bool AgreeWithDefaultDays { get; set; }
        public string Other { get; set; }
        public List<ProductService> Services { get; set; }
        public List<VendorBusinessSector> BusinessSectors { get; set; }
        public List<PrequalificationCategory> PrequalificationTypes { get; set; }
        public List<BusinessCategory> BusinessCategories { get; set; }
        public List<AttachmentDto> CompanyLogo { get; set; }
        public List<AttachmentDto> Attachments { get; set; }
    }


    public class CompanyInfo : BaseEntity
    {
        public int? VendorId { get; set; }
        public string VendorCode { get; set; }
        public int ReviseNo { get; set; }
        public string VendorName { get; set; }
        public string PhoneNo { get; set; }
        public string Email { get; set; }
        public string ContactPerson { get; set; }
        public string TaxId { get; set; }
        public string Other { get; set; }
        public string TaxOffice { get; set; }
        public string Location { get; set; }
        public string Country { get; set; }
        public string CompanyAddress { get; set; }
        public string Website { get; set; }
        public string RepresentedCompanies { get; set; }
        public string RepresentedProducts { get; set; }
        public string CompanyLogoFile { get; set; }
        public int CreditDays { get; set; }
        public string PaymentTerms { get; set; }
        public int AgreeWithDefaultDays { get; set; }
        public DateTime? CompanyRegistrationDate { get; set; }
    }
}