using PurpleRock.Application.Common.Abstractions.Implementations;

namespace PurpleRock.Application.Common.Abstractions;

/// <summary>
/// Provides extension method(s) for adding abstractions to the services container.
/// </summary>
public static class AbstractionsDependencyInjection
{
  /// <summary>
  /// Adds common abstractions to the services container.
  /// </summary>
  /// <param name="services"></param>
  /// <returns><see cref="IServiceCollection"/>.</returns>
  public static IServiceCollection AddAbstractionsDefaultImplementations(this IServiceCollection services)
  {
    services.AddSingleton<IAssemblyProvider, AssemblyProvider>();
    services.AddSingleton<IAssemblyInfoProvider, AssemblyInfoProvider>();
    services.AddSingleton<IEnvironmentValuesProvider, EnvironmentValuesProvider>();
    services.AddSingleton<IDateTimeProvider, DateTimeProvider>();
    services.AddSingleton<IGuidProvider, GuidProvider>();

    return services;
  }
}