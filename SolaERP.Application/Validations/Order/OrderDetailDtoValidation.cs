using FluentValidation;
using SolaERP.Application.Dtos.Order;

namespace SolaERP.Persistence.Validations.Order;

public class OrderDetailDtoValidation : AbstractValidator<OrderDetailDto>
{
    public OrderDetailDtoValidation()
    {
        RuleFor(x => x.ItemCode).NotEmpty()
            .NotNull()
            .WithMessage("Please, enter {PropertyName}");
    }
}