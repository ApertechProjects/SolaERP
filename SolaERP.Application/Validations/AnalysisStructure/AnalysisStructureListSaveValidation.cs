using FluentValidation;
using SolaERP.Application.Dtos.AnalysisStructure;

namespace SolaERP.Persistence.Validations.AnalysisStructure
{
    public class AnalysisStructureListSaveValidation : AbstractValidator<List<AnalysisStructureDto>>
    {
        public AnalysisStructureListSaveValidation()
        {
            RuleForEach(list => list).SetValidator(new AnalysisStructureSaveValidation());
        }
    }

    public class AnalysisStructureSaveValidation : AbstractValidator<AnalysisStructureDto>
    {
        public AnalysisStructureSaveValidation()
        {
            RuleFor(x => x.ProcedureId).NotNull().NotEmpty().WithMessage("Please, enter {PropertyName}");
            RuleFor(x => x.BusinessUnitId).NotNull().NotEmpty().WithMessage("Please, enter {PropertyName}");
            RuleFor(x => x.CatId).NotNull().NotEmpty().WithMessage("Please, enter Category");
        }
    }
}
