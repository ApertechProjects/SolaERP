namespace SolaERP.Application.Models
{
    public class RequestChangeStatusModel
    {
        public List<RequestData> RequestDatas { get; set; }
        public int ApproveStatus { get; set; }
        public string Comment { get; set; }
    }

    public class RequestData
    {
        public int RequestMainId { get; set; }
        public string RequestNo { get; set; }
        public int Sequence { get; set; }
    }

}
