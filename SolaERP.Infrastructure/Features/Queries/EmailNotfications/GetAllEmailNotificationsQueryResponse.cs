using SolaERP.Application.Entities.Email;

namespace SolaERP.Application.Features.Queries.EmailNotifications
{
    public class GetAllEmailNotificationsQueryResponse
    {
        public ICollection<EmailNotification> EmailNotifications { get; set; }
    }
}
