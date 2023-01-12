using PurpleRock.Application.Common.Exceptions;

namespace PurpleRock.Application.Common.Commands.Behaviours;

/// <summary>
/// Unhandled exception middleware.
/// </summary>
/// <typeparam name="TRequest">The incoming request.</typeparam>
/// <typeparam name="TResponse">The outgoing response.</typeparam>
internal class UnhandledExceptionBehavior<TRequest, TResponse>
  : IPipelineBehavior<TRequest, TResponse>
  where TRequest : IRequest<TResponse>
  where TResponse : IResult, new()
{
  private readonly ILogger<UnhandledExceptionBehavior<TRequest, TResponse>> _logger;

  /// <summary>
  /// Initializes a new instance of the <see cref="UnhandledExceptionBehavior{TRequest, TResponse}"/> class.
  /// </summary>
  /// <param name="logger"></param>
  public UnhandledExceptionBehavior(
    ILogger<UnhandledExceptionBehavior<TRequest, TResponse>> logger)
  {
    _logger = logger;
  }

  /// <summary>
  /// Handles unhandled exceptions.
  /// </summary>
  /// <param name="request"></param>
  /// <param name="next"></param>
  /// <param name="cancellationToken"></param>
  /// <returns></returns>
  public async Task<TResponse> Handle(
    TRequest request,
    RequestHandlerDelegate<TResponse> next,
    CancellationToken cancellationToken)
  {
    try
    {
      return await next();
    }
    catch (NotFoundException ex)
    {
      var requestName = typeof(TRequest).Name;

      _logger.LogError(ex, "Not Found Exception for Request {Name} {@Request}", requestName, request);

      return new TResponse { Status = HttpStatusCode.NotFound, CustomState = ex.Message };
    }
    catch (Exception ex) when (ex.Message.Contains("PreconditionFailed (412)"))
    {
      var requestName = typeof(TRequest).Name;

      _logger.LogError(ex, "Server conflict exception for Request {Name} {@Request}", requestName, request);

      return ServerConflictErrorResponse();
    }
    catch (Exception ex)
    {
      var requestName = typeof(TRequest).Name;

      _logger.LogError(ex, "Unhandled Exception for Request {Name} {@Request}", requestName, request);

      return InternalServerErrorResponse();
    }

    static TResponse InternalServerErrorResponse()
    {
      return new TResponse
      {
        Status = HttpStatusCode.InternalServerError,
        CustomState = "Server Error. The server encountered an error and was unable to complete your request."
      };
    }

    static TResponse ServerConflictErrorResponse()
    {
      return new TResponse
      {
        Status = HttpStatusCode.Conflict,
        CustomState =
          "Server Error: Request failed because the resource has changed on server since request. Please make your request again."
      };
    }
  }
}