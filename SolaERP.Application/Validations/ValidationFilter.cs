using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SolaERP.Application.Dtos.Shared;

namespace SolaERP.Application.Validations
{
    public class ValidationFilter : IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext context)
        {

        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            var modelState = context.ModelState;
            if (!modelState.IsValid)
            {
                var errorTex = modelState.Select(x => x.Value.Errors).Where(y => y.Count > 0).ToList();
                var propert = modelState
               .Where(x => x.Value.Errors.Count > 0)
               .Select(x => x.Key.Split(".").Last())
               .ToList();
                context.Result = new ObjectResult(ApiResponse<bool>.Fail(propert, errorTex, 422))
                {
                    StatusCode = 422,
                    Value = ApiResponse<bool>.Fail(propert, errorTex, 422),
                };
            }



            //var modelState = context.ModelState; 
            //if (!modelState.IsValid)
            //{
            //    var errorText = modelState.Select(x => x.Value.Errors).Where(y => y.Count > 0).ToList();
            //    var property = modelState
            //    .Where(x => x.Value.Errors.Count > 0)
            //    .Select(x => x.Key.Split(".").Last())
            //    .ToList()[0];
            //    context.Result = new ObjectResult(ApiResponse<bool>.Fail(property, errorText, 422));

            //}

        }
    }
}
