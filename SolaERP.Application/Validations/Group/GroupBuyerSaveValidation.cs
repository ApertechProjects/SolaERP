using FluentValidation;
using SolaERP.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolaERP.Application.Validations.GroupValidation
{
    public class GroupBuyerSaveValidation : AbstractValidator<GroupBuyerSaveModel>
    {
        public GroupBuyerSaveValidation()
        {
            RuleFor(x => x.BusinessUnitId).NotEmpty().WithMessage("Please, enter Business Unit").GreaterThanOrEqualTo(0).WithMessage("Please, enter valid Business Unit");
            RuleFor(x => x.BuyerCode).NotEmpty().WithMessage("Please, enter {PropertyName}");
        }
    }
}
