using MediatR;

namespace SolaERP.Application.Features.Commands.EmailNotifications
{
    public class DeleteEmailNotificationRequest : IRequest<bool>
    {
        public int Id { get; set; }
    }
}
