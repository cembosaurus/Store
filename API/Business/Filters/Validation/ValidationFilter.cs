﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Business.Filters.Validation
{
    public class ValidationFilter : ActionFilterAttribute
    {


        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                context.Result = new BadRequestObjectResult(context.ModelState);
            }
        }


        public void OnActionExecuted(ActionExecutedContext context) { }
    }
}
