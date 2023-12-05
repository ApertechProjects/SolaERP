using FluentValidation;
using SolaERP.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolaERP.Persistence.Validations.Invoice
{
    public class InvoiceRegisterDetailsLoadValidation : AbstractValidator<InvoiceRegisterLoadModel>
    {
        public InvoiceRegisterDetailsLoadValidation()
        {
            RuleFor(x => x.BusinessUnitId).NotEmpty().WithMessage("Please, enter {PropertyName}");
            RuleFor(x => x.Date).NotEmpty().WithMessage("Please, enter Advance Invoice");
            RuleFor(x => x.TotalAmount).NotEmpty().WithMessage("Please, enter {PropertyName}");
        }
    }
}
