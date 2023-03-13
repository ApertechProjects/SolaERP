using FluentValidation;
using SolaERP.Infrastructure.Dtos.ApproveStages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolaERP.Application.Validations.ApproveStageValidation
{
    public class ApproveStageRoleValidation : AbstractValidator<ApproveStageRoleDto>
    {
        public ApproveStageRoleValidation()
        {
            RuleFor(x => x.ApproveStageDetailId).NotEmpty().WithMessage("{PropertyName}: Please, enter {PropertyName}");
            RuleFor(x => x.ApproveRoleId).NotEmpty().WithMessage("{PropertyName}: Please, enter {PropertyName}");
            RuleFor(x => x.AmountFrom).NotEmpty().WithMessage("{PropertyName}: Please, enter {PropertyName}");
            RuleFor(x => x.AmountTo).NotEmpty().WithMessage("{PropertyName}: Please, enter {PropertyName}");
        }
    }
}
