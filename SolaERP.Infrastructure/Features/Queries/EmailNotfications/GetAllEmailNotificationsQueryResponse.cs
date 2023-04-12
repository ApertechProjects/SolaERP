using SolaERP.Infrastructure.Entities.Email;

namespace SolaERP.Infrastructure.Features.Queries.EmailNotfications
{
    public class GetAllEmailNotificationsQueryResponse
    {
        public ICollection<EmailNotfication> EmailNotfications { get; set; }
    }
}
