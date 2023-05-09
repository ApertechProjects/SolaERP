namespace SolaERP.Application.Contracts.Services
{
    public interface IEmailNotificationService
    {
        Task<string> GetCompanyName(string email);
    }
}
