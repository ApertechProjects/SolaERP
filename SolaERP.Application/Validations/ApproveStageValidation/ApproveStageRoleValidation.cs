using FluentValidation;
using SolaERP.Infrastructure.Dtos.ApproveStages;

namespace SolaERP.Application.Validations.ApproveStageValidation
{
    public class ApproveStageRoleValidation : AbstractValidator<ApproveStageRoleDto>
    {
        public ApproveStageRoleValidation()
        {
            RuleFor(x => x.ApproveStageDetailId).NotEmpty().WithMessage("Please, enter {PropertyName}");
            RuleFor(x => x.ApproveRoleId).NotEmpty().WithMessage("Please, enter {PropertyName}");
            RuleFor(x => x.AmountFrom).NotEmpty().WithMessage("Please, enter {PropertyName}");
            RuleFor(x => x.AmountTo).NotEmpty().WithMessage("Please, enter {PropertyName}");
        }
    }
}
