using Microsoft.AspNetCore.Mvc;
using SolaERP.Application.Dtos.Attachment;
using SolaERP.Application.Entities;
using SolaERP.Application.Entities.SupplierEvaluation;

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
        private DateTime _registrationDate;
        public string FullName { get; set; }
        public string Position { get; set; }
        public string PhoneNumber { get; set; }
        public string CompanyName { get; set; }
        public string TaxId { get; set; }
        public string TaxOffice { get; set; }
        public string CompanyAdress { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string WebSite { get; set; }
        public DateTime RegistrationDate
        {
            get
            {
                if (_registrationDate.Date == DateTime.MinValue)
                    _registrationDate = DateTime.Now;
                return _registrationDate;
            }
            set
            {
                _registrationDate = value;
            }
        }
        public string[] RepresentedCompanies { get; set; }
        public string[] RepresentedProducts { get; set; }
        public int CreditDays { get; set; }
        public string PaymentTerms { get; set; }
        public bool AgreeWithDefaultDays { get; set; }
        public string Other { get; set; }
        public List<ProductService> Services { get; set; }
        public List<PrequalificationCategory> PrequalificationTypes { get; set; }
        public List<BusinessCategory> BusinessCategories { get; set; }
        public List<AttachmentDto> CompanyLogo { get; set; }
        public List<AttachmentDto> Attachments { get; set; }
    }


    public class CompanyInfo : BaseEntity
    {
        public string VendorName { get; set; }
        public string TaxId { get; set; }
        public string TaxOffice { get; set; }
        public string Location { get; set; }
        public string Country { get; set; }
        public string CompanyAddress { get; set; }
        public string Website { get; set; }
        public string RepresentedCompanies { get; set; }
        public string RepresentedProducts { get; set; }
        public int CreditDays { get; set; }
        public string PaymentTerms { get; set; }
        public int AgreeWithDefaultDays { get; set; }
        public DateTime CompanyRegistrationDate { get; set; }
    }
}
