using SolaERP.Infrastructure.Entities.Email;

namespace SolaERP.Infrastructure.Features.Queries.EmailNotifications
{
    public class GetAllEmailNotificationsQueryResponse
    {
        public ICollection<EmailNotification> EmailNotifications { get; set; }
    }
}
