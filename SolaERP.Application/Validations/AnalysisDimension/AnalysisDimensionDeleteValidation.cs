using FluentValidation;
using SolaERP.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolaERP.Persistence.Validations.AnalysisDimensionValidation
{
    public class AnalysisDimensionDeleteValidation : AbstractValidator<AnalysisDimensionDeleteModel>
    {
        public AnalysisDimensionDeleteValidation()
        {
            RuleFor(x => x.DimensionIds)
            .Cascade(CascadeMode.StopOnFirstFailure)
            .NotNull().WithMessage("Please, select Analysis Dimension")
            .Must(CheckNotEqualZero.NotEqualZero).WithMessage("Please, select Analysis Dimension");
        }
    }
}
