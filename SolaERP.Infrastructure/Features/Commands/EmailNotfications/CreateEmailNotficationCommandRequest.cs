using MediatR;

namespace SolaERP.Infrastructure.Features.Commands.EmailNotifications
{
    public class CreateEmailNotificationCommandRequest : IRequest<bool>
    {
        public string Notification { get; set; }
        public string Description { get; set; }

    }
}
