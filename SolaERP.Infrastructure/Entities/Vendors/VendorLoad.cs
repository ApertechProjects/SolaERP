using SolaERP.Application.Attributes;

namespace SolaERP.Application.Entities.Vendors
{
    public class VendorLoad:BaseEntity
    {
        public int VendorId { get; set; }

        public int BlackList { get; set; }
        public string BlackListDescription { get; set; }


        public decimal Rating { get; set; }
        public int ReviseNo { get; set; }
        public DateTime? ReviseDate { get; set; }
        
        public int ApproveStatus { get; set; }
        public int Status { get; set; }


        public string VendorCode { get; set; }
        public string TaxId { get; set; }
        public string Country { get; set; }
        public string Location { get; set; } //City


        public string VendorName { get; set; }
        public string Description { get; set; }
        public string CompanyAddress { get; set; } //Address

        [DbColumn("Postal/ZIP")]
        public string Postal_ZIP { get; set; }
        public string Email { get; set; } //EmailAddress

        public string DefaultCurrency { get; set; }
        public string Website { get; set; }
        public string Address2 { get; set; }
        public string PhoneNo { get; set; } //Phone_Mobile
        public string ContactPerson { get; set; }


        public int VendorType { get; set; }
        public int ShipmentId { get; set; }
        public int DeliveryTermId { get; set; }
        public string PaymentTerms { get; set; }


        public int WithHoldingTaxId { get; set; }
        public int TaxesId { get; set; }


        public string RepresentedProducts { get; set; }
        public string RepresentedCompanies { get; set; }
        public int CreditDays { get; set; }
        [DbColumn("60DaysPayment")]
        public int _60DaysPayment { get; set; }

        public string OtherProducts { get; set; }
        public int ApproveStageMainId { get; set; }
        public DateTime CompanyRegistrationDate { get; set; }
        public string TaxOffice { get; set; }
        public string Logo { get; set; }
    }
}
