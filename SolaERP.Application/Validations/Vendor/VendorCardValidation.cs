using FluentValidation;
using SolaERP.Application.Dtos.Vendors;

namespace SolaERP.Persistence.Validations.Vendor
{
    public class VendorCardValidation : AbstractValidator<VendorCardDto>
    {
        public VendorCardValidation()
        {
            RuleFor(x => x.VendorName).NotEmpty().WithMessage("Please, enter {PropertyName}");
            RuleFor(x => x.Country).NotEmpty().WithMessage("Please, enter {PropertyName}");
        }
    }
}
