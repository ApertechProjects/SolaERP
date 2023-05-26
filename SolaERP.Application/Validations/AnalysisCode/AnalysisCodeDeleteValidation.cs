using FluentValidation;
using SolaERP.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolaERP.Persistence.Validations.AnalysisCodeValidation
{
    public class AnalysisCodeDeleteValidation : AbstractValidator<AnalysisCodeDeleteModel>
    {
        public AnalysisCodeDeleteValidation()
        {
            RuleFor(x => x.CodeIds)
            .Cascade(CascadeMode.StopOnFirstFailure)
            .NotNull().WithMessage("Please, select Analysis Code")
            .Must(CheckNotEqualZero.NotEqualZero).WithMessage("Please, select Analysis Code");
        }
    }
}
