using FluentValidation;
using SolaERP.Application.Dtos.User;

namespace SolaERP.Application.Validations.UserValidation
{
    public class UserUpdateValidation : AbstractValidator<UserUpdateDto>
    {
        public UserUpdateValidation()
        {
            RuleFor(x => x.FullName).NotEmpty().WithMessage("Please, enter {PropertyName}");
        }
    }
}
