using FluentValidation;
using SolaERP.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolaERP.Persistence.Validations.ApproveRoleValidation
{
    public class ApproveRoleDeleteValidation : AbstractValidator<ApproveRoleDeleteModel>
    {
        public ApproveRoleDeleteValidation()
        {
            RuleFor(x => x.RoleIds)
          .Cascade(CascadeMode.StopOnFirstFailure)
          .NotNull().WithMessage("Please, select Role")
          .Must(CheckNotEqualZero.NotEqualZero).WithMessage("Please, select Role");
        }
    }
}
