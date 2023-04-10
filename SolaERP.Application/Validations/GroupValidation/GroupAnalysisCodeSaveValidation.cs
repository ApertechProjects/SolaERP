using FluentValidation;
using SolaERP.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolaERP.Application.Validations.GroupValidation
{
    public class GroupAnalysisCodeSaveValidation : AbstractValidator<AnalysisCodeSaveModel>
    {
        public GroupAnalysisCodeSaveValidation()
        {
            RuleFor(x => x.GroupId).NotEmpty().WithMessage("Please, enter Group").GreaterThanOrEqualTo(0).WithMessage("Please, enter valid Group");
            RuleFor(x => x.BusinessUnitId).NotEmpty().WithMessage("Please, enter Business Unit").GreaterThanOrEqualTo(0).WithMessage("Please, enter valid Business Unit");
            RuleFor(x => x.AnalysisDimensionId).NotEmpty().WithMessage("Please, enter Analysis Dimension");
            RuleFor(x => x.AnalysisCodesId).NotEmpty().WithMessage("Please, enter Analysis Code");
        }
    }
}
