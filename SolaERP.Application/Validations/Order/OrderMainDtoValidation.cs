using FluentValidation;
using SolaERP.Application.Dtos.Order;

namespace SolaERP.Persistence.Validations.Order;

public class OrderMainDtoValidation : AbstractValidator<OrderMainDto>
{
    public OrderMainDtoValidation()
    {
        RuleForEach(x => x.OrderDetails).SetValidator(new OrderDetailDtoValidation());   
    }
}