using FluentValidation;
using SolaERP.Infrastructure.Dtos.ApproveRole;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolaERP.Application.Validations.ApproveRoleValidation
{
    public class ApproveRoleValidation : AbstractValidator<ApproveRoleDto>
    {
        public ApproveRoleValidation()
        {
            RuleFor(x => x.ApproveRoleName).NotEmpty().WithMessage("Please, enter {PropertyName}");
        }
    }
}
