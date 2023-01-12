using PurpleRock.Application.Common.Configuration;
using PurpleRock.Application.Common.Logging;

namespace PurpleRock.WebApi.Common.Extensions;

/// <content>
/// Extension methods for <see cref="IServiceCollection"/> relating to registering options.
/// </content>
public static partial class ServiceCollectionExtensions
{
  /// <summary>
  /// Configures Authentication.
  /// </summary>
  /// <param name="services"></param>
  /// <param name="configuration"></param>
  /// <returns></returns>
  public static IServiceCollection RegisterOptions(
      this IServiceCollection services,
      IConfiguration configuration)
  {
    services.AddConfigurationOptions<SerilogOptions>(configuration);
    services.AddConfigurationOptions<LoggingOptions>(configuration);
    services.AddConfigurationOptions<ServiceSettingsOptions>(configuration);
    services.AddConfigurationOptions<AppOptions>(configuration);
    services.AddConfigurationOptions<ConnectionStringsOptions>(configuration);

    return services;
  }
}