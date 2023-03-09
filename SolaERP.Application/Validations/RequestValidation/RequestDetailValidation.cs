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
            //RuleFor(x=)
        }
    }
}
