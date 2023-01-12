namespace PurpleRock.Application.Common.Models;

/// <summary>
/// Details of a Result.
/// </summary>
public interface IResult
{
  /// <summary>
  /// Stores any data required by the result.
  /// </summary>
  public object? CustomState { get; set; }

  /// <summary>
  /// Http Status code of this result.
  /// </summary>
  public HttpStatusCode Status { get; set; }
}
