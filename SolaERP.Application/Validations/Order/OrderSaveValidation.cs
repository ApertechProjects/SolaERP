using FluentValidation;
using SolaERP.Application.Dtos.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolaERP.Persistence.Validations.Order
{
    public class OrderSaveValidation : AbstractValidator<OrderMainDto>
    {
        public OrderSaveValidation()
        {
            RuleFor(x => x.DeliveryTermId).NotEmpty().WithMessage("Please, enter Delivery Term");
            RuleFor(x => x.Currency).NotEmpty().WithMessage("Please, enter {PropertyName}");
            RuleFor(x => x.PaymentTermId).NotEmpty().WithMessage("Please, enter Payment Term");
        }
    }
}
