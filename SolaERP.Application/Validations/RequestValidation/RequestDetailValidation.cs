using FluentValidation;
using SolaERP.Infrastructure.Dtos.Request;
using SolaERP.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolaERP.Application.Validations.RequestValidation
{
    public class RequestDetailValidation : AbstractValidator<RequestDetailDto>
    {
        public RequestDetailValidation()
        {
            RuleFor(x => x.ItemCode).NotEmpty().WithMessage("Please, enter {PropertyName}");
            RuleFor(x => x.RequestDeadline).NotEmpty().WithMessage("Please, enter {PropertyName}");
            RuleFor(x => x.RequestDate).NotEmpty().WithMessage("Please, enter {PropertyName}");
            //RuleFor(x => x.LineNo).NotEmpty().WithMessage("Please, enter {PropertyName}");
        }
    }
}
