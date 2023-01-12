using Microsoft.Extensions.Configuration;
using PurpleRock.Application.Common.Configuration;
using ServiceCollectionExtensions = PurpleRock.Application.Common.Extensions.ServiceCollectionExtensions;

namespace PurpleRock.Application.Common.Abstractions;

/// <summary>
/// Provides extension methods for injecting IOptions configurations.
/// </summary>
[ExcludeFromCodeCoverage]
public static class OptionsDependencyInjection
{
  /// <summary>
  /// Register Mappers Dependencies.
  /// </summary>
  /// <param name="services"></param>
  /// <param name="configuration"></param>
  /// <param name="prAssemblies"></param>
  /// <returns></returns>
  public static IServiceCollection AddApplicationOptions(
    this IServiceCollection services,
    IConfiguration configuration,
    IEnumerable<Assembly> prAssemblies)
  {
    var optionsTypes = new[] { typeof(IConfigurationOptions) };

    var regOptionsMethod = typeof(ServiceCollectionExtensions).GetMethod(
      nameof(ServiceCollectionExtensions.AddConfigurationOptions),
      BindingFlags.Static | BindingFlags.Public);

    if (regOptionsMethod == null)
      throw new InvalidOperationException("Could not find method to configure options: " +
                                          $"${nameof(ServiceCollectionExtensions.AddConfigurationOptions)}");

    // Register manual/explicit mappers
    foreach (var assembly in prAssemblies)
    foreach (var item in assembly.GetTypes())
    {
      var isConcrete = !item.IsAbstract && !item.IsInterface && !item.IsGenericType;
      if (isConcrete)
        foreach (var optionsType in optionsTypes.Where(opType => IsItemOptionsType(item, opType)))
          regOptionsMethod
            .MakeGenericMethod(item)
            .Invoke(null, new object[] { services, configuration });
    }

    static bool IsItemOptionsType(Type item, Type optionsType)
    {
      return item.GetInterfaces().Any(i => i == optionsType);
    }

    return services;
  }
}