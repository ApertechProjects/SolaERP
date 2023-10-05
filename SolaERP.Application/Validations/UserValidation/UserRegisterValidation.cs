using FluentValidation;
using SolaERP.Application.Models;

namespace SolaERP.Application.Validations.UserValidation
{
    public class UserRegisterValidation : AbstractValidator<UserRegisterModel>
    {
        public UserRegisterValidation()
        {
            RuleFor(x => x.FullName).NotEmpty().WithMessage("Please, enter {PropertyName}");
            RuleFor(x => x.UserName).NotEmpty().WithMessage("Please, enter {PropertyName}");
            RuleFor(x => x.Email).NotEmpty().WithMessage("Please, enter {PropertyName}").EmailAddress().WithMessage("Please, enter valid {PropertyName}")
                .Matches(@"^[^@\s]+@[^@\s]+\.[^@\s]+$").WithMessage("Please, enter valid {PropertyName}");
            RuleFor(x => x.Password).NotEmpty().WithMessage("Please, enter {PropertyName}").MinimumLength(7).WithMessage("Password minimum length must be 7 symbol"); ;
            RuleFor(x => x.ConfirmPassword).NotEmpty().WithMessage("Please, enter {PropertyName}").MinimumLength(7).WithMessage("Password minimum length must be 7 symbol"); ;
            RuleFor(x => x.Gender).GreaterThanOrEqualTo(1).WithMessage("Please, enter {PropertyName}").NotEmpty().WithMessage("Please, enter {PropertyName}").NotNull().ExclusiveBetween(0,3);
        }
    }
}
