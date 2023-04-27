using FluentValidation;
using SolaERP.Application.Models;

namespace SolaERP.Application.Validations.GroupValidation
{
    public class GroupAnalysisCodeSaveValidation : AbstractValidator<AnalysisCodeSaveModel>
    {
        public GroupAnalysisCodeSaveValidation()
        {
            RuleFor(x => x.GroupId).NotEmpty().WithMessage("Please, enter Group").GreaterThanOrEqualTo(0).WithMessage("Please, enter valid Group");
        }
    }
}
