using SolaERP.Application.Attributes;

namespace SolaERP.Application.Entities.RFQ
{
    public class RfqDetail
    {
        public int Id { get; set; }
        public int LineNo { get; set; }
        public string ItemCode { get; set; }
        public string ItemCategory { get; set; }
        public string Description { get; set; }
        public string UOM { get; set; }
        public decimal Quantity { get; set; }
        public Guid GUID { get; set; }

        [NotInclude]
        public List<RfqRequestDetail> LineDetails { get; set; }
    }


    public class RfqRequestDetail
    {
        public int Id { get; set; }
        public int RequestDetailId { get; set; }
        public decimal Quantity { get; set; }
        public string UOM { get; set; }
        public Guid GUID { get; set; }

    }
}
