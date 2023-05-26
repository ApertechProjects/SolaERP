using FluentValidation;
using SolaERP.Application.Models;
using SolaERP.Persistence.Validations.ApproveRoleValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolaERP.Persistence.Validations.AnalysisCodeValidation
{
    public class AnalysisCodeListSaveValidation : AbstractValidator<List<AnalysisCodeSaveModel>>
    {
        public AnalysisCodeListSaveValidation()
        {
            RuleForEach(list => list).SetValidator(new AnalysisCodeSaveValidation());
        }
    }

    public class AnalysisCodeSaveValidation : AbstractValidator<AnalysisCodeSaveModel>
    {
        public AnalysisCodeSaveValidation()
        {
            RuleFor(x => x.AnalysisCode).NotNull().NotEmpty().WithMessage("Please, enter {PropertyName}");
            RuleFor(x => x.AnalysisDimensionId).NotNull().NotEmpty().WithMessage("Please, enter {PropertyName}");
            RuleFor(x => x.AnalysisName).NotNull().NotEmpty().WithMessage("Please, enter {PropertyName}");
        }
    }
}
