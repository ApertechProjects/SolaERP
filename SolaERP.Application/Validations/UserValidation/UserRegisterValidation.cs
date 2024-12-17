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
            RuleFor(x => x.PhoneNumber)
                        .NotEmpty()
                        .WithMessage("Please, enter {PropertyName}")
                        .When(x => x.UserTypeId == 0);
            RuleFor(x => x.TaxId)
                       .NotEmpty()
                       .WithMessage("Please, enter {PropertyName}")
                       .When(x => x.UserTypeId == 0);
            RuleFor(x => x.Email).NotEmpty().WithMessage("Please, enter {PropertyName}").EmailAddress().WithMessage("Please, enter valid {PropertyName}")
                .Matches(@"^[^@\s]+@[^@\s]+\.[^@\s]+$").WithMessage("Please, enter valid {PropertyName}");

            RuleFor(x => x.Password)
			.NotEmpty()
			.WithMessage("Password is required.")
			.MinimumLength(7)
			.WithMessage("Password must be at least 8 characters long.")
			.Matches("[A-Z]")
			.WithMessage("Password must contain at least one uppercase letter.")
			.Matches("[a-z]")
			.WithMessage("Password must contain at least one lowercase letter.")
			.Matches("[0-9]")
			.WithMessage("Password must contain at least one number.")
			.Matches("[^a-zA-Z0-9]")
			.WithMessage("Password must contain at least one special character.");

		
            RuleFor(x => x.Gender).GreaterThanOrEqualTo(1).WithMessage("Please, enter {PropertyName}").NotEmpty().WithMessage("Please, enter {PropertyName}").NotNull().ExclusiveBetween(0, 3);
        }
    }
}
