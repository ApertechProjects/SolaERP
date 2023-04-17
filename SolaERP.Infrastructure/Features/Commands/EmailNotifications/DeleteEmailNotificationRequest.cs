using MediatR;

namespace SolaERP.Infrastructure.Features.Commands.EmailNotifications
{
    public class DeleteEmailNotificationRequest : IRequest<bool>
    {
        public int Id { get; set; }
    }
}
