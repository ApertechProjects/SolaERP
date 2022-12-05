using FluentValidation;
using SolaERP.Infrastructure.Dtos.User;

namespace SolaERP.Application.Validations.UserValidation
{
    public class UserUpdatePasswordValidation : AbstractValidator<UserUpdatePasswordDto>
    {
        public UserUpdatePasswordValidation()
        {
            RuleFor(x => x.Password).MinimumLength(7);
            RuleFor(x => x.ConfirmPassword).MinimumLength(7);
        }
    }
}
