

namespace SolaERP.Application.Entities.RFQ
{
    public class RFQInProgress : RFQBase
    {

        public int OfferCount { get; set; }
        public bool Sent { get; set; }
        public bool Accepted { get; set; }
        public bool InProgress { get; set; }
        public bool Responded { get; set; }
        public bool Rejected { get; set; }
        public bool NoResponse { get; set; }
        public string BusinessCategoryName { get; set; }
    }
}
