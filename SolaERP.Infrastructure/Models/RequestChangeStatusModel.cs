namespace SolaERP.Infrastructure.Models
{
    public class RequestChangeStatusModel
    {
        public List<int> RequestMainIds { get; set; }
        public int ApproveStatus { get; set; }
        public string Comment { get; set; }
    }
}
