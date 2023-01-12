namespace PurpleRock.Application.Common.Commands.Behaviours;

/// <summary>
/// Validation middleware.
/// </summary>
/// <typeparam name="TRequest">The incoming request.</typeparam>
/// <typeparam name="TResponse">The outgoing response.</typeparam>
internal class ValidationBehavior<TRequest, TResponse>
  : IPipelineBehavior<TRequest, TResponse>
  where TRequest : IRequest<TResponse>
  where TResponse : IResult, new()
{
  private readonly ILogger<ValidationBehavior<TRequest, TResponse>> _logger;
  private readonly IEnumerable<IValidator<TRequest>> _validators;

  /// <summary>
  /// Initializes a new instance of the <see cref="ValidationBehavior{TRequest, TResponse}"/> class.
  /// </summary>
  /// <param name="validators"></param>
  /// <param name="logger"></param>
  public ValidationBehavior(
    IEnumerable<IValidator<TRequest>> validators,
    ILogger<ValidationBehavior<TRequest, TResponse>> logger)
  {
    _validators = validators;
    _logger = logger;
  }

  /// <summary>
  /// Handles Validation.
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
    var errors = _validators
      .Select(v => v.Validate(request))
      .SelectMany(vr => vr.Errors)
      .GroupBy(x => x.PropertyName)
      .ToDictionary(x => x.Key, x => x.Select(y => y.ErrorMessage).ToArray());

    if (!errors.Any()) return await next();

    _logger.LogInformation($"Validation failed: {string.Join(" ", errors.Select(e => e.Value))}");

    var validation = new ValidationProblemDetails(errors);

    return new TResponse
    {
      Status = HttpStatusCode.BadRequest,
      CustomState = validation
    };
  }
}