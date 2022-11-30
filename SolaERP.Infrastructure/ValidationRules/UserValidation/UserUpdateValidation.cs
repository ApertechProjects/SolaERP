using FluentValidation;
using SolaERP.Infrastructure.Dtos.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolaERP.Infrastructure.ValidationRules.UserValidation
{
    public class UserUpdateValidation:AbstractValidator<UserUpdateDto>
    {
        public UserUpdateValidation()
        {
            RuleFor(x => x.FullName).NotEmpty().WithMessage("Please, enter {PropertyName}");
        }
    }
}
