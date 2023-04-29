using FluentValidation;
using SolaERP.Application.Models;

namespace SolaERP.Persistence.Validations.UserValidation
{
    public class UserSaveModelValidation : AbstractValidator<UserSaveModel>
    {
        public UserSaveModelValidation()
        {
            RuleFor(x => x.UserName).NotEmpty().WithMessage("Please, enter {PropertyName}").MinimumLength(3);
            RuleFor(x => x.FullName).NotEmpty().WithMessage("Please, enter {PropertyName}");
            RuleFor(x => x.Email).NotEmpty().WithMessage("Please, enter {PropertyName}");
            RuleFor(x => x.Password).NotEmpty().WithMessage("Please, enter {PropertyName}").MinimumLength(7);
            RuleFor(x => x.Gender).NotEmpty().WithMessage("Please, enter {PropertyName}");
            RuleFor(x => x.VendorId).NotEmpty().When(x => x.UserTypeId == 0).WithMessage("Please, enter {PropertyName}");
            RuleFor(x => x.Buyer).NotEmpty().When(x => x.UserTypeId == 1).WithMessage("Please, enter {PropertyName}");
            When(x => x.UserTypeId == 0, () =>
            {
                RuleFor(b => b.Buyer)
                .Empty()
                .WithMessage("A user cannot be both a Vendor and a Buyer simultaneously.");
            });
        }
    }
}
