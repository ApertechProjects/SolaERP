namespace SolaERP.Infrastructure.Models
{
    public class RequestMainApproveModel
    {
        public int UserId { get; set; }
        public List<int> RequestMainId { get; set; }
        public int ApproveStatus { get; set; }
        public string Comment { get; set; }
    }
}
