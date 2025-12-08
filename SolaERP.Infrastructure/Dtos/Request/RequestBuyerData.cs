namespace SolaERP.Application.Dtos.Request;

public class RequestBuyerData
{
        public int RequestMainId { get; set; }
        public string BuyerName { get; set; }
        public string Email { get; set; }
        public string RequestNo { get; set; }
        public string BusinessUnitName { get; set; }
        public int RequestBuyerResponseId { get; set; }
        public string Language { get; set; }
        public int UserId { get; set; } = 0;
    
}