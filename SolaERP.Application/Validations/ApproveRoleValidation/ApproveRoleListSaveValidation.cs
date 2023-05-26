using FluentValidation;
using SolaERP.Application.Models;
using SolaERP.Application.Validations.ApproveStageValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolaERP.Persistence.Validations.ApproveRoleValidation
{
    public class ApproveRoleListSaveValidation : AbstractValidator<List<ApproveRoleSaveModel>>
    {
        public ApproveRoleListSaveValidation()
        {
            RuleForEach(list => list).SetValidator(new ApproveRoleSaveValidation());
        }
    }

    public class ApproveRoleSaveValidation : AbstractValidator<ApproveRoleSaveModel>
    {
        public ApproveRoleSaveValidation()
        {
            RuleFor(x => x.ApproveRoleName).NotNull().NotEmpty().WithMessage("Please, enter {PropertyName}");
            RuleFor(x => x.BusinessUnitId).NotNull().NotEmpty().WithMessage("Please, enter {PropertyName}");
        }
    }
}
