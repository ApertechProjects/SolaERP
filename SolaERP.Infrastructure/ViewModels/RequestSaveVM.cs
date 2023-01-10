using SolaERP.Infrastructure.Dtos.Request;

namespace SolaERP.Infrastructure.ViewModels
{
    public class RequestSaveVM
    {
        public RequestMainDto RequestMainDto { get; set; }
        public List<RequestDetailDto> RequestDetailDtos { get; set; }
    }
}
