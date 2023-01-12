using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace PurpleRock.WebApi.Common.Health;

/// <summary>
/// Custom response writer for Health Checks.
/// </summary>
internal static class HealthCheckResponseWriter
{
  /// <summary>
  /// Writes the contents of a <see cref="HealthReport"/> to the provided <see cref="HttpContext"/> as a JSON response.
  /// </summary>
  /// <param name="context">The <see cref="HttpContext"/> to write the response to.</param>
  /// <param name="healthReport">The <see cref="HealthReport"/> to present as JSON.</param>
  /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
  public static async Task WriteResponseAsJsonAsync(HttpContext context, HealthReport healthReport)
  {
    if (healthReport.Status == HealthStatus.Healthy)
    {
      context.Response.ContentType = "text/plain; charset=utf-8";
      await context.Response.WriteAsync("Healthy");
      return;
    }

    var healthCheckResponse = Map(healthReport);

    var json = JsonSerializer.Serialize(
        healthCheckResponse,
        new JsonSerializerOptions
        {
          PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
          DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        });

    context.Response.ContentType = "application/json; charset=utf-8";
    await context.Response.WriteAsync(json);
  }

  /// <summary>
  /// Map the <see cref="HealthReport"/> received to the <see cref="HealthCheckResponse"/> to return.
  /// </summary>
  /// <param name="healthReport">The health data.</param>
  /// <returns>The mapped response.</returns>
  internal static HealthCheckResponse Map(HealthReport healthReport)
  {
    return new HealthCheckResponse
    {
      Status = healthReport.Status.ToString(),
      TotalDurationSeconds = healthReport.TotalDuration.TotalSeconds.ToString("F3"),
      Results = healthReport.Entries.Select(e =>
          new HealthCheckResponseEntry
          {
            Name = e.Key,
            Description = e.Value.Description,
            DurationSeconds = e.Value.Duration.TotalSeconds.ToString("F3"),
            Status = e.Value.Status.ToString(),
            Exception = e.Value.Exception?.Message,
            Data = e.Value.Data
          })
    };
  }
}
