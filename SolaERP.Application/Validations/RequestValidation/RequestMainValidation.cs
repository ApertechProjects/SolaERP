using FluentValidation;
using SolaERP.Infrastructure.Models;

namespace SolaERP.Application.Validations.RequestValidation
{
    public class RequestMainValidation : AbstractValidator<RequestSaveModel>
    {
        public RequestMainValidation()
        {
            RuleFor(x => x.RequestTypeId).NotNull().WithMessage("Please, enter {PropertyName}").GreaterThanOrEqualTo(0).WithMessage("Please enter valid data");
            RuleFor(x => x.BusinessUnitId).NotNull().WithMessage("Please, enter {PropertyName}").GreaterThanOrEqualTo(0).WithMessage("Please enter valid data");
            RuleFor(x => x.EntryDate).NotNull().WithMessage("Please, enter {PropertyName}");
            RuleFor(x => x.RequestDate).NotNull().WithMessage("Please, enter {PropertyName}");
            RuleFor(x => x.RequestDeadline).NotNull().WithMessage("Please, enter {PropertyName}");
            RuleFor(x => x.Requester).NotNull().WithMessage("Please, enter {PropertyName}");
        }
    }
}

