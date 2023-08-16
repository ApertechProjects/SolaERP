using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolaERP.Application.Features
{
    public class ListModelBinder<T> : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            if (bindingContext == null)
            {
                throw new ArgumentNullException(nameof(bindingContext));
            }

            var values = bindingContext.ValueProvider.GetValue(bindingContext.ModelName).ToString();
            var list = JsonConvert.DeserializeObject<List<T>>(values);

            bindingContext.Result = ModelBindingResult.Success(list);
            return Task.CompletedTask;
        }
    }
}
