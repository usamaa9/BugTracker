using Destructurama;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace PurpleRock.Application.Common.Extensions;

/// <summary>
/// Extensions to handle IHostBuilder configuration.
/// </summary>
[ExcludeFromCodeCoverage]
public static class HostBuilderExtensions
{
  /// <summary>
  /// Configures the Functions builder with the configuration settings.
  /// </summary>
  /// <param name="builder"></param>
  /// <returns></returns>
  public static IHostBuilder UsePurpleRockLogging(this IHostBuilder builder)
  {
    builder
        .UseSerilog((context, services, loggerConfiguration) =>
        {
          // Configure Serilog setup issues to be reported to console
          Serilog.Debugging.SelfLog.Enable(Console.Error);

          // Configure Serilog from appconfig and services
          loggerConfiguration
              .ReadFrom.Configuration(context.Configuration)
              .ReadFrom.Services(services)
              .Destructure.UsingAttributes();
        });

    return builder;
  }
}