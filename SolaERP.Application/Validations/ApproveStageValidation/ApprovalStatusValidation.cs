using FluentValidation;
using SolaERP.Infrastructure.Dtos.ApproveStages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolaERP.Application.Validations.ApproveStageValidation
{
    public class ApprovalStatusValidation : AbstractValidator<ApprovalStatusDto>
    {
        public ApprovalStatusValidation()
        {
            RuleFor(x => x.ApprovalStatusName).NotEmpty().WithMessage("{PropertyName}: Please, enter {PropertyName}");
        }
    }
}
