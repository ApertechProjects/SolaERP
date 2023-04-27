using MediatR;

namespace SolaERP.Application.Features.Commands.EmailNotifications
{
    public class CreateEmailNotificationCommandRequest : IRequest<bool>
    {
        public string Notification { get; set; }
        public string Description { get; set; }

    }
}
