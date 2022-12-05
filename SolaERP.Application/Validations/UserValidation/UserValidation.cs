using FluentValidation;
using SolaERP.Infrastructure.Dtos.UserDto;

namespace SolaERP.Application.Validations.UserValidation
{
    public class UserValidation : AbstractValidator<UserDto>
    {
        public UserValidation()
        {
            RuleFor(x => x.UserName).NotEmpty().WithMessage("Please, enter {PropertyName}");
            RuleFor(x => x.FullName).NotEmpty().WithMessage("Please, enter {PropertyName}");
            RuleFor(x => x.Email).NotEmpty().WithMessage("Please, enter {PropertyName}");
            RuleFor(x => x.Password).NotEmpty().WithMessage("Please, enter {PropertyName}").MinimumLength(7);
            RuleFor(x => x.ConfirmPassword).NotEmpty().WithMessage("Please, enter {PropertyName}").MinimumLength(7);

        }
    }
}
