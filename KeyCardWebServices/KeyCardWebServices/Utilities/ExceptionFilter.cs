using KeyCardWebServices.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace KeyCardWebServices.Utilities;

public class ExceptionFilter : IActionFilter, IOrderedFilter
{
    public int Order => int.MaxValue;

    private readonly ILogger<ExceptionFilter> _logger;

    public ExceptionFilter(ILogger<ExceptionFilter> logger)
    {
        _logger = logger;
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
        if (context.Exception != null && !context.ExceptionHandled)
        {
            _logger.LogError(context.Exception, "An unhandled error occured in controller {controller}", (context.Controller as ControllerBase)?.ToString());
            context.Result = context.HttpContext.Request.Handle(context.Exception);

            context.ExceptionHandled = true;
        }
    }

    public void OnActionExecuting(ActionExecutingContext context)
    {
        
    }
}
