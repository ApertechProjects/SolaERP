using FluentValidation;
using SolaERP.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolaERP.Persistence.Validations.Invoice
{
    public class InvoiceRegisterApproveValidation : AbstractValidator<InvoiceRegisterApproveModel>
    {
        public InvoiceRegisterApproveValidation()
        {
            RuleFor(x=>x.BusinessUnitId).NotEmpty().WithMessage("Please, enter {PropertyName}");
        }
    }
}
