using FluentValidation;
using SolaERP.Application.Dtos.SupplierEvaluation;
using SolaERP.Application.Models;

namespace SolaERP.Persistence.Validations.Supplier
{
    public class SupplierRegisterValidation : AbstractValidator<SupplierRegisterCommand>
    {
        public SupplierRegisterValidation()
        {
            RuleFor(x => x.CompanyInformation).NotNull().NotEmpty()
                   .SetValidator(new CompanyValidation());

            RuleForEach(x => x.BankAccounts).NotEmpty().NotNull()
                .SetValidator(new BankAccountsValidation());
        }
    }
    public class BankAccountsValidation : AbstractValidator<VendorBankDetailDto>
    {
        public BankAccountsValidation()
        {
            RuleFor(x => x.AccountNumber).NotNull().NotEmpty().WithMessage("Please, enter {PropertyName}");
            RuleFor(x => x.Address).NotNull().NotEmpty().WithMessage("Please, enter {PropertyName}");
            RuleFor(x => x.Bank).NotNull().NotEmpty().WithMessage("Please, enter {PropertyName}");
            RuleFor(x => x.CoresspondentAccount).NotNull().NotEmpty().WithMessage("Please, enter {PropertyName}");
            RuleFor(x => x.Currency).NotNull().NotEmpty().WithMessage("Please, enter {PropertyName}");
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
        }

    }


}