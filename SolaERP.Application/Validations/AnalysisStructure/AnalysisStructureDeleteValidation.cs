using FluentValidation;
using SolaERP.Application.Dtos.AnalysisStructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolaERP.Persistence.Validations.AnalysisStructure
{
    public class AnalysisStructureDeleteValidation : AbstractValidator<AnalysisStructureDto>
    {
        public AnalysisStructureDeleteValidation()
        {
           // RuleFor(x => x.)
           //.Cascade(CascadeMode.StopOnFirstFailure)
           //.NotNull().WithMessage("Please, select Analysis Dimension")
           //.Must(CheckNotEqualZero.NotEqualZero).WithMessage("Please, select Analysis Structure");
        }
    }
}
