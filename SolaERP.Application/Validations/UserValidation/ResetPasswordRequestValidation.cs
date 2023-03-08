using FluentValidation;
using SolaERP.Infrastructure.Models;

namespace SolaERP.Application.Validations.UserValidation
{
    public class ResetPasswordRequestValidation : AbstractValidator<ResetPasswordModel>
    {
        public ResetPasswordRequestValidation()
        {
            RuleFor(x => x.Email).NotNull().NotEmpty().MaximumLength(80);
            RuleFor(x => x.Password).MinimumLength(7);
            RuleFor(x => x.ConfirmPassword).MinimumLength(7);
        }
    }
}
