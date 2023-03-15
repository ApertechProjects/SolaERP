using FluentValidation;
using SolaERP.Infrastructure.Dtos.Auth;

namespace SolaERP.Application.Validations.UserValidation
{
    public class LoginValidation : AbstractValidator<LoginRequestModel>
    {
        public LoginValidation()
        {
            RuleFor(x => x.Email).NotEmpty().WithMessage("Please, enter {PropertyName}");
            RuleFor(x => x.Password).NotEmpty().WithMessage("Please, enter {PropertyName}").MinimumLength(7).WithMessage("Password minimum length must be 7 symbol");
        }
    }
}
