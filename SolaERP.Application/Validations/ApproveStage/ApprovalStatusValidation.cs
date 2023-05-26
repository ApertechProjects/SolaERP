using FluentValidation;
using SolaERP.Application.Dtos.ApproveStages;

namespace SolaERP.Application.Validations.ApproveStageValidation
{
    public class ApprovalStatusValidation : AbstractValidator<ApprovalStatusDto>
    {
        public ApprovalStatusValidation()
        {
            RuleFor(x => x.ApproveStageDetailsName).NotEmpty().WithMessage("Please, enter {PropertyName}");
        }
    }
}
