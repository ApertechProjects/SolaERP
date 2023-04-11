using FluentValidation;
using SolaERP.Infrastructure.Dtos.ApproveStages;

namespace SolaERP.Application.Validations.ApproveStageValidation
{
    public class ApproveStageDetailValidation : AbstractValidator<ApproveStagesDetailDto>
    {
        public ApproveStageDetailValidation()
        {
            //RuleFor(x => x.ApproveStageMainId).NotEmpty().WithMessage("Please, enter {PropertyName}");
            RuleFor(x => x.ApproveStageDetailsName).NotEmpty().WithMessage("Please, enter {PropertyName}");
            RuleFor(x => x.Sequence).NotEmpty().WithMessage("Please, enter {PropertyName}");

            RuleForEach(x => x.ApproveStageRolesDto).SetValidator(new ApproveStageRoleValidation());
        }
    }
}
