using SolaERP.Infrastructure.Dtos.Shared;
using SolaERP.Infrastructure.Dtos.User;

namespace SolaERP.Infrastructure.Contracts.Services
{
    public interface IEmailService
    {
        ApiResponse<bool> SendEmailForResetPassword(UserCheckVerifyCodeDto dto);
    }
}
