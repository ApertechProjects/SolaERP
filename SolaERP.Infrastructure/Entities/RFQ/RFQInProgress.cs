

namespace SolaERP.Application.Entities.RFQ
{
    public class RFQInProgress : RFQBase
    {
        public int OfferCount { get; set; }
        public int Sent { get; set; }
        public int Accepted { get; set; }
        public int InProgress { get; set; }
        public int Responded { get; set; }
        public int Rejected { get; set; }
        public int NoResponse { get; set; }
        public string BusinessCategoryName { get; set; }
        public DateTime SentDate { get; set; }
    }
}
