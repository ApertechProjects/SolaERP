using FluentValidation;
using SolaERP.Infrastructure.Dtos.User;

namespace SolaERP.Application.Validations.UserValidation
{
    public class ResetPasswordRequestValidation : AbstractValidator<ResetPasswordRequestDto>
    {
        public ResetPasswordRequestValidation()
        {
            RuleFor(x => x.Email).NotNull().NotEmpty().MaximumLength(80);
            RuleFor(x => x.Password).MinimumLength(7);
            RuleFor(x => x.ConfirmPassword).MinimumLength(7);
        }
    }
}
