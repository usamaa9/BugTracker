using Microsoft.Extensions.Configuration;
using PurpleRock.Application.Common.Configuration;

namespace PurpleRock.Application.Common.Extensions;

/// <summary>
/// Extension methods for <see cref="IServiceCollection"/> relating to Binding Configuration.
/// </summary>
[ExcludeFromCodeCoverage]
public static class ServiceCollectionExtensions
{
  /// <summary>
  /// Register configuration options of the specified type from <see cref="IConfiguration"/>.
  /// </summary>
  /// <typeparam name="TOptions">The type of settings to bind to.</typeparam>
  /// <param name="services"></param>
  /// <param name="configuration"></param>
  public static void AddConfigurationOptions<TOptions>(this IServiceCollection services, IConfiguration configuration)
      where TOptions : class, IConfigurationOptions, new()
  {
    var sectionName = new TOptions().SectionName;

    if (sectionName is null)
    {
      services.AddOptions<TOptions>()
          .Bind(configuration)
          .ValidateDataAnnotations()
          .ValidateOnStart();
    }
    else
    {
      services.AddOptions<TOptions>()
          .Bind(configuration.GetSection(sectionName))
          .ValidateDataAnnotations()
          .ValidateOnStart();
    }
  }
}