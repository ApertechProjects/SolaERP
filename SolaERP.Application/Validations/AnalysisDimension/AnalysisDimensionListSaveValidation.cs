using FluentValidation;
using SolaERP.Application.Dtos.AnaysisDimension;
using SolaERP.Application.Models;
using SolaERP.Persistence.Validations.AnalysisCodeValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolaERP.Persistence.Validations.AnalysisDimensionValidation
{
    public class AnalysisDimensionListSaveValidation : AbstractValidator<List<AnalysisDimensionDto>>
    {
        public AnalysisDimensionListSaveValidation()
        {
            RuleForEach(list => list).SetValidator(new AnalysisDimensionSaveValidation());
        }

    }
    public class AnalysisDimensionSaveValidation : AbstractValidator<AnalysisDimensionDto>
    {
        public AnalysisDimensionSaveValidation()
        {
            RuleFor(x => x.AnalysisDimensionName).NotNull().NotEmpty().WithMessage("Please, enter {PropertyName}");
            RuleFor(x => x.AnalysisDimensionCode).NotNull().NotEmpty().WithMessage("Please, enter {PropertyName}");
            RuleFor(x => x.BusinessUnitId).NotNull().NotEmpty().WithMessage("Please, enter {PropertyName}");
            RuleFor(x => x.MinLength).NotNull().NotEmpty().WithMessage("Please, enter {PropertyName}");
            RuleFor(x => x.MaxLength).NotNull().NotEmpty().WithMessage("Please, enter {PropertyName}");
        }
    }
}
