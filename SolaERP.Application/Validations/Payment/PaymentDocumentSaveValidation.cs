using FluentValidation;
using SolaERP.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolaERP.Persistence.Validations.Payment
{
    public class PaymentDocumentSaveValidation : AbstractValidator<PaymentDocumentSaveModel>
    {
        //public PaymentDocumentSaveValidation()
        //{
        //    RuleFor(x => x.Main.VendorCode).NotEmpty().WithMessage("Please, enter Vendor Code");
        //    RuleFor(x => x.Main.CurrencyCode).NotEmpty().WithMessage("Please, enter Currency Code");
        //    RuleFor(x => x.Main.BusinessUnitId).NotEmpty().WithMessage("Please, enter Business Unit");
        //    RuleFor(x => x.Main.ApproveStageMainId).NotEmpty().WithMessage("Please, enter Approve Stage");
        //    RuleFor(x => x.Main.PaymentDocumentPriorityId).NotEmpty().WithMessage("Please, enter Payment Document Priority");
        //    RuleFor(x => x.Main.PaymentAttachmentTypeId).NotEmpty().WithMessage("Please, enter Payment Document Type");
        //    RuleFor(x => x.Main.SentDate).NotEmpty().WithMessage("Please, enter Sent Date");
        //}
    }
}
