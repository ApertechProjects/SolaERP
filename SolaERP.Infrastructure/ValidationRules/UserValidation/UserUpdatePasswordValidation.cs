using FluentValidation;
using SolaERP.Infrastructure.Dtos.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolaERP.Infrastructure.ValidationRules.UserValidation
{
    public class UserUpdatePasswordValidation : AbstractValidator<UserUpdatePasswordDto>
    {
        public UserUpdatePasswordValidation()
        {
            RuleFor(x => x.PasswordHash).MinimumLength(7);
            RuleFor(x => x.ConfirmPasswordHash).MinimumLength(7);
        }
    }
}
