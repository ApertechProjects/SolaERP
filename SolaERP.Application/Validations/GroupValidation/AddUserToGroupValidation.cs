using FluentValidation;
using SolaERP.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolaERP.Application.Validations.GroupValidation
{
    public class AddUserToGroupValidation : AbstractValidator<AddUserToGroupModel>
    {
        public AddUserToGroupValidation()
        {
            //RuleFor(x => x.GroupId).NotEmpty().WithMessage("Please, enter {PropertyName}").GreaterThanOrEqualTo(0).WithMessage("Please, enter {PropertyName}");
            //RuleFor(x => x.UserId).NotEmpty().WithMessage("Please, enter {PropertyName}").GreaterThanOrEqualTo(0).WithMessage("Please, enter {PropertyName}");
        }
    }
}
