using FluentValidation;
using SolaERP.Infrastructure.Dtos.ApproveStage;

namespace SolaERP.Application.Validations.ApproveStageValidation
{
    public class ApproveStageMainValidation : AbstractValidator<ApproveStagesMainDto>
    {
        public ApproveStageMainValidation()
        {
            RuleFor(x => x.ProcedureId).NotEmpty().WithMessage("Please, enter {PropertyName}");
            RuleFor(x => x.BusinessUnitId).NotEmpty().WithMessage("Please, enter {PropertyName}");
            RuleFor(x => x.ApproveStageName).NotEmpty().WithMessage("Please, enter {PropertyName}");
        }
    }
}
