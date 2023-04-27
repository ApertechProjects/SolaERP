using MediatR;

namespace SolaERP.Application.Features.Commands.EmailNotifications
{
    public class UpdateEmailNotificationRequest : IRequest<bool>
    {
        public int EmailNotificationId { get; set; }
        public string Notification { get; set; }
        public string Description { get; set; }
    }
}
