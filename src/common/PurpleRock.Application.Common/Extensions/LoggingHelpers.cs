using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Events;
using Serilog.Extensions.Hosting;

namespace PurpleRock.Application.Common.Extensions;

/// <summary>
/// Logger configuration helpers.
/// </summary>
public static class LoggingHelpers
{
  /// <summary>
  /// Application Insights Connection string key.
  /// </summary>
  public static string AppInsightsConnectionStringKey { get; } = "APPLICATIONINSIGHTS_CONNECTION_STRING";

  /// <summary>
  /// Startup Logger configuration writing to Console and Application Insights
  /// if connection string provided as APPLICATIONINSIGHTS_CONNECTION_STRING.
  /// </summary>
  /// <returns></returns>
  [ExcludeFromCodeCoverage]
  public static ReloadableLogger GetBootstrapLogger()
  {
    var config = new ConfigurationBuilder().LoadDefaultAndEnvironmentVariables();

#pragma warning disable IDE0079 // Remove unnecessary suppression
#pragma warning disable S4792 // This logger configuration has been reviewed and is safe

    var loggerConfiguration = new LoggerConfiguration()
        .MinimumLevel.Override("Microsoft", minimumLevel: LogEventLevel.Information)
        .MinimumLevel.Override("System", LogEventLevel.Information)
        .Enrich.FromLogContext()
        .WriteTo.Console();

#pragma warning restore S4792, IDE0079

    var connectionString = config.GetValue(AppInsightsConnectionStringKey, string.Empty);
    if (!string.IsNullOrEmpty(connectionString))
    {
      loggerConfiguration.WriteTo.ApplicationInsights(connectionString, TelemetryConverter.Events);
    }

    return loggerConfiguration.CreateBootstrapLogger();
  }
}