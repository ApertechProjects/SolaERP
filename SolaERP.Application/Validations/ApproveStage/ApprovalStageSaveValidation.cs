using FluentValidation;
using SolaERP.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolaERP.Application.Validations.ApproveStageValidation
{
    public class ApprovalStageSaveValidation : AbstractValidator<ApprovalStageSaveModel>
    {
        public ApprovalStageSaveValidation()
        {
            RuleFor(x => x.ApproveStagesMainDto).SetValidator(new ApproveStageMainValidation());
            RuleForEach(x => x.ApproveStagesDetailDtos).SetValidator(new ApproveStageDetailValidation());
        }
    }

}
