using SolaERP.Infrastructure.Entities.Email;

namespace SolaERP.Infrastructure.Contracts.Repositories
{
    public interface IEmailNotficationRepository
    {
        Task<List<EmailNotfication>> GetAllEmailNotficationsAsync();
        Task<bool> CreateAsync(EmailNotfication model);
        Task<bool> UpdateAsync(EmailNotfication model);
        Task<bool> DeleteAsync(int id);

    }
}
