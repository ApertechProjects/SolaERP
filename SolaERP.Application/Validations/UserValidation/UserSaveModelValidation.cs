using FluentValidation;
using SolaERP.Application.Models;

namespace SolaERP.Persistence.Validations.UserValidation
{
    public class UserSaveModelValidation : AbstractValidator<UserSaveModel>
    {
        public UserSaveModelValidation()
        {
            RuleFor(x => x.UserTypeId)
                .NotEmpty()
                .WithMessage("Please, enter {PropertyName}");
            RuleFor(x => x.UserName)
                .NotEmpty()
                .WithMessage("Please, enter {PropertyName}")
                .MinimumLength(3);
            RuleFor(x => x.FullName)
                .NotEmpty()
                .WithMessage("Please, enter {PropertyName}");
            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress()
                .WithMessage("Please, enter {PropertyName}");
            RuleFor(x => x.Gender)
                .NotEmpty()
                .WithMessage("Please, enter {PropertyName}");
            RuleFor(x => x.Password)
                .NotEmpty()
                .WithMessage("Please, enter {PropertyName}")
                .MinimumLength(7);
            RuleFor(x => x.ConfirmPassword)
                .NotEmpty()
                .WithMessage("Please, enter {PropertyName}")
                .Equal(m => m.Password)
                .WithMessage("confirm Password:  Confirm Password doesn't match the Password!");
            RuleFor(x => x.VendorId)
                .NotEmpty()
                .When(x => x.UserTypeId == 1)
                .WithMessage("Please, enter {PropertyName}");
            RuleFor(x => x.Gender)
                .GreaterThan(0)
                .LessThan(3)
                .WithMessage("Gender can't be {PropertyValue}. Male:1 Female:2");
        }
    }
}
