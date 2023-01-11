namespace SolaERP.Infrastructure.Dtos.Request
{
    public class RequestMainWithDetailsDto : RequestMainDto
    {
        public List<RequestDetailDto> Details { get; set; }
    }
}
