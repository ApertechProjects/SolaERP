using FluentValidation;
using SolaERP.Application.Models;

namespace SolaERP.Persistence.Validations.UserValidation
{
    public class UserSaveModelValidation : AbstractValidator<UserSaveModel>
    {
        public UserSaveModelValidation()
        {
            When(x => x.UserTypeId == 0, () =>
            {
                RuleFor(x => x.VendorId)
                .NotEmpty()
                .WithMessage("Please, enter {PropertyName}");
            });
            RuleFor(x => x.UserName)
                .NotEmpty()
                .WithMessage("Please, enter {PropertyName}")
                .MinimumLength(3);
            RuleFor(x => x.FullName)
                .NotEmpty()
                .WithMessage("Please, enter {PropertyName}");
            RuleFor(x => x.Email)
                .NotEmpty()
                .WithMessage("Please, enter {PropertyName}")
                .EmailAddress()
                .WithMessage("Please enter valid email format");
            RuleFor(x => x.Gender)
                .NotEmpty()
                .WithMessage("Please, enter {PropertyName}");
            RuleFor(x => x.Password)
                .NotEmpty()
                .WithMessage("Please, enter {PropertyName}")
                .MinimumLength(7)
                .WithMessage("The length of '{PropertyName}' must be at least {MinLength} characters. You entered {TotalLength} characters.");
            RuleFor(x => x.ConfirmPassword)
                .NotEmpty()
                .WithMessage("Please, enter {PropertyName}")
                .Equal(m => m.Password)
                .WithMessage("confirm Password:  Confirm Password doesn't match the Password!");
            RuleFor(x => x.Gender)
                .GreaterThan(0)
                .LessThan(3)
                .WithMessage("Gender can't be {PropertyValue}. Male:1 Female:2");
        }
    }
}
