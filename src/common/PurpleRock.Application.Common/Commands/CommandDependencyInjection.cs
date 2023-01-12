using PurpleRock.Application.Common.Commands.Behaviours;

namespace PurpleRock.Application.Common.Commands;

/// <summary>
/// Provides extension method(s) for adding command bus to the services container.
/// </summary>
[ExcludeFromCodeCoverage]
public static class CommandDependencyInjection
{
  /// <summary>
  /// Adds command framework to the services container.
  /// </summary>
  /// <typeparam name="TCommandBus"><see cref="ICommandBus"/>.</typeparam>
  /// <param name="services"></param>
  /// <param name="assemblies"></param>
  /// <returns><see cref="IServiceCollection"/>.</returns>
  public static IServiceCollection AddCommandFramework<TCommandBus>(
    this IServiceCollection services,
    Assembly[] assemblies)
    where TCommandBus : class, ICommandBus
  {
    services.AddScoped<CommandItemsCache>();
    services.AddTransient<ICommandBus, TCommandBus>();

    services.AddValidatorsFromAssemblies(assemblies, includeInternalTypes: true);

    services.AddMediatR(assemblies);
    services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
    services.AddTransient(typeof(IPipelineBehavior<,>), typeof(UnhandledExceptionBehavior<,>));
    services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

    return services;
  }
}