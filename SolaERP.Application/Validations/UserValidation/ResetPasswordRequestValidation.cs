using FluentValidation;
using SolaERP.Infrastructure.Models;

namespace SolaERP.Application.Validations.UserValidation
{
    public class ResetPasswordRequestValidation : AbstractValidator<ResetPasswordModel>
    {
        public ResetPasswordRequestValidation()
        {
            RuleFor(x => x.ResetPasswordCode).NotNull().NotEmpty().WithMessage("Please, enter {PropertyName}").MaximumLength(80);
            RuleFor(x => x.Password).NotEmpty().WithMessage("Please enter {PropertyName}").MinimumLength(7).WithMessage("Password minimum length must be 7 symbol");
            RuleFor(x => x.ConfirmPassword).NotEmpty().WithMessage("Please enter {PropertyName}").MinimumLength(7).WithMessage("Password minimum length must be 7 symbol");
        }
    }
}
