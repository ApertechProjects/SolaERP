using FluentValidation;
using SolaERP.Application.Dtos.Vendors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolaERP.Persistence.Validations.Vendor
{
    public class VendorCardValidation : AbstractValidator<VendorCardDto>
    {
        public VendorCardValidation()
        {
            RuleFor(x => x.VendorName).NotEmpty().WithMessage("Please, enter {PropertyName}");
            RuleFor(x => x.TaxId).NotEmpty().WithMessage("Please, enter {PropertyName}");
            RuleFor(x => x.Country).NotEmpty().WithMessage("Please, enter {PropertyName}");
            RuleFor(x => x.Tax).NotEmpty().WithMessage("Please, enter Taxes");
        }
    }
}
