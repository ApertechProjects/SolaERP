using SolaERP.Application.Attributes;
using SolaERP.Application.Entities.SupplierEvaluation;
using SolaERP.Application.Enums;
using SolaERP.Application.Models;

namespace SolaERP.Application.Entities.RFQ
{
    public class RfqDetailSaveModel
    {
        public int Id { get; set; }
        public int LineNo { get; set; }
        public string ItemCode { get; set; }
        public int BusinessCategoryId { get; set; }
        public string Description { get; set; }
        public string UOM { get; set; }
        public decimal Quantity { get; set; }
        public Guid GUID { get; set; }
        public int Condition { get; set; }
        public bool AlternativeItem { get; set; }
        public decimal ConversionRate { get; set; }
        [NotInclude]
        public List<AttachmentSaveModel> Attachments { get; set; }
        [NotInclude]
        public List<int> DeletedRequestDetailIds { get; set; }

        [NotInclude]
        public List<RfqRequestDetailSaveModel> RequestDetails { get; set; }
    }


    public class RfqRequestDetailSaveModel
    {
        public int Id { get; set; }
        public int RequestDetailId { get; set; }
        public decimal Quantity { get; set; }
        public string UOM { get; set; }
        public Guid GUID { get; set; }
        public int BusinessCategoryId { get; set; }

    }

    public class RFQDetail
    {
        public int Id { get; set; }
        public int RFQMainId { get; set; }
        public int LineNo { get; set; }
        public string ItemCode { get; set; }
        public string ItemName1 { get; set; }
        public string ItemName2 { get; set; }
        public BusinessCategory BusinessCategory { get; set; }
        public Condition Condition { get; set; } = Condition.Any;
        public bool AlternativeItem { get; set; }
        public string UOM { get; set; }
        public string DefaultUOM { get; set; }
        public decimal Quantity { get; set; }
        public decimal CONV_ID { get; set; }
        public string Description { get; set; }
        public Guid GUID { get; set; }
        public List<RFQRequestDetail> RequestDetails { get; set; }
    }



    public class RFQRequestDetail
    {
        //public long RowNum { get; set; }
        public int Id { get; set; }
        public int RFQDetailId { get; set; }
        public int RequestDetailId { get; set; }
        public string RequestNo { get; set; }
        public string RequestLine { get; set; }
        public int RFQLine { get; set; }
        public string ItemCode { get; set; }
        public string ItemName1 { get; set; }
        public string ItemName2 { get; set; }
        public decimal Quantity { get; set; }
        public string UOM { get; set; }
        public string DefaultUOM { get; set; }
        public Condition Condition { get; set; }
        public string Buyer { get; set; }
        public BusinessCategory BusinessCategory { get; set; }
        public decimal CONV_ID { get; set; }
        public Guid GUID { get; set; }
        public bool AlternativeItem { get; set; }
        public decimal RequestQuantity { get; set; }
    }
}
