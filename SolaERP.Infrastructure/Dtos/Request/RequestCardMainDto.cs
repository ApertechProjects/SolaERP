using SolaERP.Application.Dtos.Attachment;

namespace SolaERP.Application.Dtos.Request
{
    public class RequestCardMainDto
    {
        private int _status;
        private int _priority;
        private int _destination;
        public int RequestMainId { get; set; }
        public int BusinessUnitId { get; set; }
        public string Buyer { get; set; }
        public string BusinessUnitCode { get; set; }
        public string BusinessUnitName { get; set; }
        public int RequestTypeId { get; set; }
        public string KeyCode { get; set; }
        public string RequestNo { get; set; }
        public int Destination
        {
            get
            {
                if (RequestMainId == 0)
                    _destination = 1;
                return _destination;
            }
            set
            {
                _destination = value;
            }
        }
        public string ApproveStatus { get; set; }
        public string AccountCode { get; set; }
        public string AccountName { get; set; }
        public DateTime EntryDate { get; set; } = DateTime.Now.Date;
        public DateTime RequestDate { get; set; }
        public DateTime RequestDeadline { get; set; }
        public int UserId { get; set; }
        public int Requester { get; set; }
        public int Status
        {
            get
            {
                if (RequestMainId == 0)
                    _status = 1;
                return _status;
            }
            set
            {
                _status = value;
            }
        }
        public string RequestComment { get; set; }
        public string OperatorComment { get; set; }
        public string QualityRequired { get; set; }
        public string CurrencyCode { get; set; }
        public decimal LogisticsTotal { get; set; }
        public string PotentialVendor { get; set; }
        public int Priority
        {
            get
            {
                if (RequestMainId == 0)
                    _priority = 1;
                return _priority;
            }
            set
            {
                _priority = value;
            }
        }
        public int ApproveStageMainId { get; set; }
        public List<RequestCardDetailDto> requestCardDetails { get; set; }
        public List<RequestCardAnalysisDto> requestCardAnalysis { get; set; }
        public List<AttachmentDto> Attachments { get; set; }

    }
}
