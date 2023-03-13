using FluentValidation;
using SolaERP.Infrastructure.Dtos.ApproveStages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolaERP.Application.Validations.ApproveStageValidation
{
    public class ApproveStageDetailValidation : AbstractValidator<ApproveStagesDetailDto>
    {
        public ApproveStageDetailValidation()
        {
            RuleFor(x => x.ApproveStageMainId).NotEmpty().WithMessage("{PropertyName}: Please, enter {PropertyName}");
            RuleFor(x => x.ApproveStageDetailsName).NotEmpty().WithMessage("{PropertyName}: Please, enter {PropertyName}");
            RuleFor(x => x.Sequence).NotEmpty().WithMessage("{PropertyName}: Please, enter {PropertyName}");

            RuleForEach(x => x.ApproveStageRolesDto).SetValidator(new ApproveStageRoleValidation());
        }
    }
}
