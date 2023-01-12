namespace PurpleRock.Application.Common.Commands.Behaviours;

/// <summary>
/// Logging middleware.
/// </summary>
/// <typeparam name="TRequest">The incoming request.</typeparam>
/// <typeparam name="TResponse">The outgoing response.</typeparam>
internal class LoggingBehavior<TRequest, TResponse>
  : IPipelineBehavior<TRequest, TResponse>
  where TRequest : IRequest<TResponse>
{
  private readonly ILogger<LoggingBehavior<TRequest, TResponse>> _logger;

  /// <summary>
  /// Initializes a new instance of the <see cref="LoggingBehavior{TRequest, TResponse}"/> class.
  /// </summary>
  /// <param name="logger"></param>
  public LoggingBehavior(ILogger<LoggingBehavior<TRequest, TResponse>> logger)
  {
    _logger = logger;
  }

  /// <summary>
  /// Handles logging.
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
    var requestTypeName = typeof(TRequest).Name;

    _logger.LogInformation($"[Handling request]: {requestTypeName}");

    var response = await next();

    _logger.LogInformation($"[Request handled]: {requestTypeName}");

    return response;
  }
}