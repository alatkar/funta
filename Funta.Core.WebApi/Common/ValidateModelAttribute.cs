using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Funta.Core.Web.Api.Common
{
    public class ValidateModelAttribute : ActionFilterAttribute
    {
        /*
         
              In Startup.cs ConfigureServices
                    services.AddMvc(opts =>
                    {
                        // apply [ValidateModel] globally
                        opts.Filters.Add(typeof(ValidateModelAttribute));
                    });
             */
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                context.Result = new BadRequestObjectResult(context.ModelState); // returns 400 with error
            }
        }
    }
}
