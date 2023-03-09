using FluentValidation;
using SolaERP.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolaERP.Application.Validations.RequestValidation
{
    public class RequestMainValidation : AbstractValidator<RequestSaveModel>
    {
        public RequestMainValidation()
        {
            RuleFor(x => x.RequestTypeId).NotNull().WithMessage("{PropertyName}: Please enter {PropertyName}").GreaterThanOrEqualTo(0).WithMessage("{PropertyName}: Please enter valid data");
            RuleFor(x => x.BusinessUnitId).NotNull().WithMessage("{PropertyName}: Please enter {PropertyName}").GreaterThanOrEqualTo(0).WithMessage("{PropertyName}: Please enter valid data");
            RuleFor(x => x.EntryDate).NotNull().WithMessage("{PropertyName}: Please enter {PropertyName}");
            RuleFor(x => x.RequestDate).NotNull().WithMessage("{PropertyName}: Please enter {PropertyName}");
            RuleFor(x => x.RequestDeadline).NotNull().WithMessage("{PropertyName}: Please enter {PropertyName}");
            RuleFor(x => x.Requester).NotNull().WithMessage("{PropertyName}: Please enter {PropertyName}");
        }
    }
}

