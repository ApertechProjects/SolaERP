using FluentValidation;
using SolaERP.Infrastructure.Dtos.UserDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolaERP.Infrastructure.ValidationRules.UserValidation
{
    public class UserValidation : AbstractValidator<UserDto>
    {
        public UserValidation()
        {
            RuleFor(x => x.UserName).NotEmpty().WithMessage("Please, enter {PropertyName}");
            RuleFor(x => x.FullName).NotEmpty().WithMessage("Please, enter {PropertyName}");
            RuleFor(x => x.Email).NotEmpty().WithMessage("Please, enter {PropertyName}");
            RuleFor(x => x.PasswordHash).NotEmpty().WithMessage("Please, enter {PropertyName}");
            RuleFor(x => x.ConfirmPasswordHash).NotEmpty().WithMessage("Please, enter {PropertyName}").MinimumLength(7);
            RuleFor(x => x.PasswordHash).MinimumLength(7);
        }
    }
}
