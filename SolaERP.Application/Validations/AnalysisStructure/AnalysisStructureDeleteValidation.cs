using FluentValidation;
using SolaERP.Application.Dtos.AnalysisStructure;
using SolaERP.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolaERP.Persistence.Validations.AnalysisStructure
{
    public class AnalysisStructureDeleteValidation : AbstractValidator<AnalysisStructureDeleteModel>
    {
        public AnalysisStructureDeleteValidation()
        {
            RuleFor(x => x.StructureIds)
            .Cascade(CascadeMode.StopOnFirstFailure)
            .NotNull().WithMessage("Please, select Analysis Structure")
            .Must(CheckNotEqualZero.NotEqualZero).WithMessage("Please, select Analysis Structure");
        }
    }
}
