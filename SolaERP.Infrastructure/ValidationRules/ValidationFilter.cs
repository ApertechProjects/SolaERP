using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SolaERP.Infrastructure.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolaERP.Infrastructure.ValidationRules
{
    public class ValidationFilter : IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext context)
        {

        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            var modelState = context.ModelState;
            var errorText = modelState.Select(x => x.Value.Errors).Where(y => y.Count > 0).ToList()[0][0].ErrorMessage;
            if (!modelState.IsValid)
            {
                context.Result = new ObjectResult(ApiResponse<bool>.Fail(errorText, 400));
            }
        }
    }
}
