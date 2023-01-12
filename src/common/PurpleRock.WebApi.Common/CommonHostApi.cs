using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using PurpleRock.Application.Common.Extensions;
using Serilog;

namespace PurpleRock.WebApi.Common;

/// <summary>
/// Provides common API for building, configuring and hosting an application.
/// </summary>
public static class CommonHostApi
{
  /// <summary>
  /// Runs an application by instantiating a HostBuilder with common functionality and executing optional pre and post host creation hooks.
  /// </summary>
  /// <typeparam name="TStartupType">Startup file type (Commonly the Startup.cs file).</typeparam>
  /// <param name="args">Startup arguments.</param>
  /// <param name="preCreateHostBuilderHook">Any actions to execute before the HostBuilder is created.</param>
  /// <param name="postCreateHostBuilderHook">Any actions to execute after the HostBuilder is created, before it is built and ran.</param>
  public static void Run<TStartupType>(
      string[] args,
      Action? preCreateHostBuilderHook = null,
      Func<IHostBuilder, IHostBuilder>? postCreateHostBuilderHook = null)
      where TStartupType : class
  {
    Log.Logger = LoggingHelpers.GetBootstrapLogger();

    try
    {
      Log.Information("Executing hook before CreateHostBuilder");
      preCreateHostBuilderHook?.Invoke();

      Log.Information("Creating Host builder");
      var hostBuilder = CreateHostBuilder<TStartupType>(args);

      if (postCreateHostBuilderHook != null)
      {
        Log.Information("Executing hook after CreateHostBuilder");
        hostBuilder = postCreateHostBuilderHook(hostBuilder);
      }

      Log.Information("Starting up");
      hostBuilder.Build().Run();
    }
    catch (Exception ex)
    {
      Log.Fatal(ex, "Application startup failed.");
    }
    finally
    {
      // Safely close the log streams
      Log.CloseAndFlush();
    }
  }

  private static IHostBuilder CreateHostBuilder<TStartupType>(string[] args)
      where TStartupType : class
  {
    var hostBuilder = Host
        .CreateDefaultBuilder(args)
        .ConfigureWebHostDefaults(builder => builder.UseStartup<TStartupType>())
        .ConfigureAppConfiguration((hostBuilderContext, builder) =>
            builder.LoadPurpleRockConfiguration(hostBuilderContext.HostingEnvironment.EnvironmentName))
        .UsePurpleRockLogging();

    return hostBuilder;
  }
}