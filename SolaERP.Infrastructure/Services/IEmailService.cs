using SolaERP.Infrastructure.Dtos;
using SolaERP.Infrastructure.Dtos.User;
using SolaERP.Infrastructure.Entities.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolaERP.Infrastructure.Services
{
    public interface IEmailService
    {
        bool ValidateEmail(UserResetPasswordDto dto);
        ApiResponse<bool> SendEmailForResetPassword(UserResetPasswordDto dto);
        ApiResponse<bool> VerifyIncomingCodeFromMail(string verifyCode);
    }
}
