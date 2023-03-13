using FluentValidation;
using SolaERP.Infrastructure.Models;

namespace SolaERP.Application.Validations.UserValidation
{
    public class ResetPasswordRequestValidation : AbstractValidator<ResetPasswordModel>
    {
        public ResetPasswordRequestValidation()
        {
            RuleFor(x => x.Email).NotNull().NotEmpty().WithMessage("{PropertyName}: Please enter {PropertyName}").MaximumLength(80).WithMessage("{PropertyName}: Email length can not be greater than 80 symbol");
            RuleFor(x => x.Password).NotEmpty().WithMessage("{PropertyName}: Please enter password").MinimumLength(7).WithMessage("{ProperyName}: Password minimum length must be 7 symbol");
            RuleFor(x => x.ConfirmPassword).NotEmpty().WithMessage("{PropertyName}: Please enter password").MinimumLength(7).WithMessage("{ProperyName}: Password minimum length must be 7 symbol");
        }
    }
}
