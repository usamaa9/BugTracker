namespace PurpleRock.WebApi.Common.Health;

/// <summary>
/// Information about the health of a WebAPI.
/// </summary>
internal class HealthCheckResponse
{
  /// <summary>
  /// The overall status of the health check.
  /// </summary>
  public string? Status { get; set; }

  /// <summary>
  /// Amount of time that the response took to execute.
  /// </summary>
  public string? TotalDurationSeconds { get; set; }

  /// <summary>
  /// Health check details.
  /// </summary>
  public IEnumerable<HealthCheckResponseEntry>? Results { get; set; }
}
