using KeyCardWebServices.Extensions;
using Microsoft.AspNetCore.Mvc.Filters;

namespace KeyCardWebServices.Utilities;

public class ExceptionFilter : IActionFilter, IOrderedFilter
{
    public int Order => int.MaxValue;

    public void OnActionExecuted(ActionExecutedContext context)
    {
        if (context.Exception != null && !context.ExceptionHandled)
        {
            context.Result = context.HttpContext.Request.Handle(context.Exception);

            context.ExceptionHandled = true;
        }
    }

    public void OnActionExecuting(ActionExecutingContext context)
    {
        
    }
}
