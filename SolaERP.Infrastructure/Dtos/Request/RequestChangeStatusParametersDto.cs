namespace SolaERP.Infrastructure.Dtos.Request
{
    public class RequestChangeStatusParametersDto
    {
        public int RequestMainId { get; set; }
        public int RequestDetailId { get; set; }
        public string FinderToken { get; set; }
        public int Sequence { get; set; }
        public int UserId { get; set; }
        public int Status { get; set; }
        public int ApproveStatus { get; set; }
        public string Comment { get; set; }
    }
}
