using FluentValidation;
using SolaERP.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolaERP.Infrastructure.ValidationRules.UserValidation
{
    public class UserCheckVerifyCodeDtoValidation:AbstractValidator<UserCheckVerifyCodeModel>
    {
        public UserCheckVerifyCodeDtoValidation()
        {
            RuleFor(x=>x.Email).NotEmpty().WithMessage("Please, enter {PropertyName}");
        }
    }
}
