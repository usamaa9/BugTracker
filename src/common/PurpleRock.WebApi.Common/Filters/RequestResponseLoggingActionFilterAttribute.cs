using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using PurpleRock.Application.Common.Extensions;

namespace PurpleRock.WebApi.Common.Filters;

/// <summary>
/// The Request Response Logging Action Filter.
/// </summary>
[ExcludeFromCodeCoverage]
internal sealed class RequestResponseLoggingActionFilterAttribute : ActionFilterAttribute
{
  /// <summary>
  /// Executed before a request is passed to .Net Core controllers.
  /// </summary>
  /// <param name="context"></param>
  public override void OnActionExecuting(ActionExecutingContext context)
  {
    var logger = (ILogger<RequestResponseLoggingActionFilterAttribute>)context
        .HttpContext.RequestServices.GetService(
            typeof(ILogger<RequestResponseLoggingActionFilterAttribute>))!;

    if (context.ActionArguments.Any(x => x.Value?.GetType().IsSimple() == false))
    {
      var model = context.ActionArguments.FirstOrDefault(x => x.Value?.GetType().IsSimple() == false);
      logger.LogInformation("Request model: {@value}", model.Value);
    }
    else
    {
      logger.LogInformation("No request model");
    }

    base.OnActionExecuting(context);
  }

  /// <summary>
  /// Executed when .Net Core is returning the result to the consumer.
  /// </summary>
  /// <param name="context"></param>
  public override void OnResultExecuting(ResultExecutingContext context)
  {
    var logger = (ILogger<RequestResponseLoggingActionFilterAttribute>)context
        .HttpContext.RequestServices.GetService(
            typeof(ILogger<RequestResponseLoggingActionFilterAttribute>))!;

    if (context.Result is ObjectResult result)
    {
      logger.LogInformation("Response model: {@value}", result.Value);
    }
    else
    {
      logger.LogInformation("No response model");
    }

    base.OnResultExecuting(context);
  }
}