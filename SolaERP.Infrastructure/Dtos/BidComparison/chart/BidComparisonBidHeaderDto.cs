namespace SolaERP.Application.Dtos.BidComparison;

public class BidComparisonBidHeaderDto
{
        public int bidMainId { get; set; }
        public string bidNo { get; set; }
        public string vendorId { get; set; }
        public string vendorName { get; set; }
        public string vendorWHTRate { get; set; }
        public string vendorTaxRate { get; set; }
        public string disqualified { get; set; }
        public string disqualificationReason { get; set; }
        public string deliveryTermId { get; set; }
        public string deliveryTermCode { get; set; }
        public string deliveryTermName { get; set; }
        public string deliveryTermScore { get; set; }
        public string deliveryTime { get; set; }
        public string paymentTermId { get; set; }
        public string paymentTermCode { get; set; }
        public string paymentTermName { get; set; }
        public string paymentTermScore { get; set; }
        public string expectedCost { get; set; }
        public string budgetBalance { get; set; }
        public string currencyId { get; set; }
        public string currencyName { get; set; }
        public string currencyKey { get; set; }
        public Decimal total { get; set; }
        public Decimal discount { get; set; }
        public Decimal discountedAmount { get; set; }
        public string baseAmount { get; set; }
        public string reportingAmount { get; set; }
        public string baseCurrencyId { get; set; }
        public string baseCurrencyKey { get; set; }
        public string reportingCurrencyId { get; set; }
        public string sentDate { get; set; }
        public string vendorCode { get; set; }
        public string whtRate { get; set; }
        public string totalWithWHT { get; set; }
        public string taxValue { get; set; }
        public string grantTotal { get; set; }
        public string grantTotalAZN { get; set; }
        public string createdDate { get; set; }
        public string businessUnitId { get; set; }
        public string warranty { get; set; }
        public string operatorComment { get; set; }
        public string hasBankGuarantee { get; set; }
        public List<BidComparisonBidDetailsDto> BidDetails { get; set; }

}