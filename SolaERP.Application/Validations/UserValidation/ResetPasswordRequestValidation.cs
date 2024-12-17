using FluentValidation;
using SolaERP.Application.Models;

namespace SolaERP.Application.Validations.UserValidation
{
    public class ResetPasswordRequestValidation : AbstractValidator<ResetPasswordModel>
    {
        public ResetPasswordRequestValidation()
        {
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
		}
    }
}
