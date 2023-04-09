using FluentValidation;
using SolaERP.Infrastructure.Models;

namespace SolaERP.Application.Validations.UserValidation
{
    public class UserRegisterValidation : AbstractValidator<UserRegisterModel>
    {
        public UserRegisterValidation()
        {
            RuleFor(x => x.FullName).NotEmpty().WithMessage("Please, enter {PropertyName}");
            RuleFor(x => x.UserName).NotEmpty().WithMessage("Please, enter {PropertyName}");
            RuleFor(x => x.Email).NotEmpty().WithMessage("Please, enter {PropertyName}").EmailAddress().WithMessage("Please, enter valid {PropertyName}");
            RuleFor(x => x.Password).NotEmpty().WithMessage("Please, enter {PropertyName}");
            RuleFor(x => x.ConfirmPassword).NotEmpty().WithMessage("Please, enter {PropertyName}");
            RuleFor(x => x.Gender).GreaterThanOrEqualTo(0).WithMessage("Please, enter {PropertyName}").NotEmpty().WithMessage("Please, enter {PropertyName}");
            RuleFor(x => x.UserType).NotEmpty().WithMessage("Please, enter {PropertyName}");

        }
    }
}
