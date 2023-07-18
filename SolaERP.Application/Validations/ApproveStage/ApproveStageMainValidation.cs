using FluentValidation;
using SolaERP.Application.Dtos.ApproveStage;
using SolaERP.Application.Models;

namespace SolaERP.Application.Validations.ApproveStageValidation
{
    public class ApproveStageMainValidation : AbstractValidator<ApproveStageMainInputModel>
    {
        public ApproveStageMainValidation()
        {
            RuleFor(x => x.ProcedureId).NotEmpty().WithMessage("Please, enter {PropertyName}");
            RuleFor(x => x.ApproveStageName).NotEmpty().WithMessage("Please, enter {PropertyName}");
            RuleFor(x => x.ApproveStageCode).NotEmpty().WithMessage("Please, enter {PropertyName}");
        }
    }
}
