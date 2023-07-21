using FluentValidation;
using SolaERP.Application.Dtos.ApproveStages;

namespace SolaERP.Application.Validations.ApproveStageValidation
{
    public class ApproveStageRoleValidation : AbstractValidator<ApprovalStageRoleDto>
    {
        public ApproveStageRoleValidation()
        {
            //RuleFor(x => x.AmountFrom).NotEmpty().WithMessage("Please, enter {PropertyName}");
            //RuleFor(x => x.AmountTo).NotEmpty().WithMessage("Please, enter {PropertyName}");
        }
    }
}
