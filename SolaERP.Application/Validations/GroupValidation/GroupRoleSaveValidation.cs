using FluentValidation;
using SolaERP.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolaERP.Application.Validations.GroupValidation
{
    public class GroupRoleSaveValidation : AbstractValidator<GroupRoleSaveModel>
    {
        public GroupRoleSaveValidation()
        {
            RuleFor(x => x.GroupId).NotEmpty().WithMessage("Please, enter Group").GreaterThanOrEqualTo(0).WithMessage("Please, enter valid Group");
            RuleFor(x => x.ApproveRoleId).NotEmpty().WithMessage("Please, enter Approve Role").GreaterThanOrEqualTo(0).WithMessage("Please, enter valid Approve Role");
         
        }
    }
}
