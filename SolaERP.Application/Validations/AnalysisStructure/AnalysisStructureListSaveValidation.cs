using FluentValidation;
using SolaERP.Application.Dtos.AnalysisStructure;
using SolaERP.Application.Dtos.AnaysisDimension;
using SolaERP.Persistence.Validations.AnalysisDimensionValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolaERP.Persistence.Validations.AnalysisStructure
{
    public class AnalysisStructureListSaveValidation:AbstractValidator<List<AnalysisStructureDto>>
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
            RuleFor(x => x.AnalysisDimensionid1).NotNull().NotEmpty().WithMessage("Please, enter {PropertyName}");
            RuleFor(x => x.AnalysisDimensionid2).NotNull().NotEmpty().WithMessage("Please, enter {PropertyName}");
            RuleFor(x => x.AnalysisDimensionid3).NotNull().NotEmpty().WithMessage("Please, enter {PropertyName}");
            RuleFor(x => x.AnalysisDimensionid4).NotNull().NotEmpty().WithMessage("Please, enter {PropertyName}");
            RuleFor(x => x.AnalysisDimensionid5).NotNull().NotEmpty().WithMessage("Please, enter {PropertyName}");
            RuleFor(x => x.AnalysisDimensionid6).NotNull().NotEmpty().WithMessage("Please, enter {PropertyName}");
            RuleFor(x => x.AnalysisDimensionid7).NotNull().NotEmpty().WithMessage("Please, enter {PropertyName}");
            RuleFor(x => x.AnalysisDimensionid8).NotNull().NotEmpty().WithMessage("Please, enter {PropertyName}");
            RuleFor(x => x.AnalysisDimensionid9).NotNull().NotEmpty().WithMessage("Please, enter {PropertyName}");
            RuleFor(x => x.AnalysisDimensionid10).NotNull().NotEmpty().WithMessage("Please, enter {PropertyName}");
        }
    }
}
