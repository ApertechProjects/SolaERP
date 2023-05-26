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
          
        }
    }
}
