namespace SolaERP.Infrastructure.Dtos.Request
{
    public class RequestFollowDto
    {
        public int RequestFollowId { get; set; }
        public int UserId { get; set; }
        public string FullName { get; set; }
        public int RequestMainId { get; set; }
    }
}
