using MediatR;

namespace SolaERP.Infrastructure.Features.Commands.EmailNotfications
{
    public class UpdateEmailNotficationRequest : IRequest<bool>
    {
        public int EmailNotficationId { get; set; }
        public string Notfication { get; set; }
        public string Description { get; set; }
    }
}
