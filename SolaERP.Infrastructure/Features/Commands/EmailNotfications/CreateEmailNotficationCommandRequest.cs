using MediatR;

namespace SolaERP.Infrastructure.Features.Commands.EmailNotfications
{
    public class CreateEmailNotficationCommandRequest : IRequest<bool>
    {
        public string Notfication { get; set; }
        public string Description { get; set; }

    }
}
