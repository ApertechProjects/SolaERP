using FluentValidation;
using SolaERP.Infrastructure.Dtos.ApproveStage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolaERP.Application.Validations.ApproveStageValidation
{
    public class ApproveStageMainValidation : AbstractValidator<ApproveStagesMainDto>
    {
        public ApproveStageMainValidation()
        {
            RuleFor(x => x.ProcedureId).NotEmpty().WithMessage("{PropertyName}: Please, enter {PropertyName}");
            RuleFor(x => x.BusinessUnitId).NotEmpty().WithMessage("{PropertyName}: Please, enter {PropertyName}");
            RuleFor(x => x.ApproveStageName).NotEmpty().WithMessage("{PropertyName}: Please, enter {PropertyName}");
        }
    }
}
