using FluentValidation;
using SolaERP.Application.Dtos.SupplierEvaluation;
using SolaERP.Application.Models;

namespace SolaERP.Persistence.Validations.Supplier
{
    public class SupplierRegisterValidation : AbstractValidator<SupplierRegisterCommand>
    {
        public SupplierRegisterValidation()
        {
            RuleFor(x => x.CompanyInformation)
                   .SetValidator(new CompanyValidation());
        }
    }

    public class CompanyValidation : AbstractValidator<CompanyInfoDto>
    {
        public CompanyValidation()
        {
            RuleFor(x => x.CompanyName).NotNull().NotEmpty().WithMessage("Please, enter {PropertyName}");
            RuleFor(x => x.TaxId).NotNull().NotEmpty().WithMessage("Please, enter {PropertyName}");
            RuleFor(x => x.CompanyAdress).NotNull().NotEmpty().WithMessage("Please, enter {PropertyName}");
            RuleFor(x => x.City).NotNull().NotEmpty().WithMessage("Please, enter {PropertyName}");
            RuleFor(x => x.Country).NotNull().NotEmpty().WithMessage("Please, enter {PropertyName}");
            RuleFor(x => x.CreditDays).NotNull().NotEmpty().WithMessage("Please, enter {PropertyName}");
        }

    }
}