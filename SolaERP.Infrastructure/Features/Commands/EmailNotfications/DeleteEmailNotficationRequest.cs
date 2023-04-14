using MediatR;

namespace SolaERP.Infrastructure.Features.Commands.EmailNotfications
{
    public class DeleteEmailNotficationRequest : IRequest<bool>
    {
        public int Id { get; set; }
    }
}
