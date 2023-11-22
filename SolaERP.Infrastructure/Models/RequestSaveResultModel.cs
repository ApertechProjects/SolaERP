namespace SolaERP.Application.Models
{
    public class RequestSaveResultModel
    {
        public int RequestMainId { get; set; }
        public string RequestNo { get; set; }
        public List<int> RequestDetailIds { get; set; }
    }
}
