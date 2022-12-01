using FluentValidation;
using SolaERP.Infrastructure.Dtos.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolaERP.Infrastructure.ValidationRules.UserValidation
{
    public class UserCheckVerifyCodeDtoValidation:AbstractValidator<UserCheckVerifyCodeDto>
    {
        public UserCheckVerifyCodeDtoValidation()
        {
            RuleFor(x=>x.Email).NotEmpty().WithMessage("Please, enter {PropertyName}");
        }
    }
}
