using FluentValidation;
using SolaERP.Application.Dtos.SupplierEvaluation;
using SolaERP.Application.Models;

namespace SolaERP.Persistence.Validations.Supplier
{
    public class SupplierRegisterValidation : AbstractValidator<SupplierRegisterCommand>
    {
        public SupplierRegisterValidation()
        {
            RuleFor(x => x.CompanyInfo)
                   .SetValidator(new CompanyValidation());

            RuleForEach(x => x.BankAccounts).SetValidator(new BankAccountsValidation());
        }
    }
    public class BankAccountsValidation : AbstractValidator<BankAccountsDto>
    {
        public BankAccountsValidation()
        {
            RuleFor(x => x.BankDetails.AccountNumber).NotNull().NotEmpty().WithMessage("Please, enter {PropertyName}");
            RuleFor(x => x.BankDetails.Address).NotNull().NotEmpty().WithMessage("Please, enter {PropertyName}");
            RuleFor(x => x.BankDetails.Bank).NotNull().NotEmpty().WithMessage("Please, enter {PropertyName}");
            RuleFor(x => x.BankDetails.CoresspondentAccount).NotNull().NotEmpty().WithMessage("Please, enter {PropertyName}");
            RuleFor(x => x.BankDetails.Currency).NotNull().NotEmpty().WithMessage("Please, enter {PropertyName}");
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