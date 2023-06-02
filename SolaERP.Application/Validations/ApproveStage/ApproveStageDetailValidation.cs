using FluentValidation;
using SolaERP.Application.Dtos.ApproveStages;
using SolaERP.Application.Models;

namespace SolaERP.Application.Validations.ApproveStageValidation
{
    public class ApproveStageDetailValidation : AbstractValidator<ApproveStageDetailInputModel>
    {
        public ApproveStageDetailValidation()
        {
            RuleFor(x => x.ApproveStageDetailsName).NotEmpty().WithMessage("Please, enter {PropertyName}");
            RuleFor(x => x.Sequence).NotEmpty().WithMessage("Please, enter {PropertyName}");

            RuleForEach(x => x.ApproveStageRoles).SetValidator(new ApproveStageRoleValidation());
        }
    }
}
