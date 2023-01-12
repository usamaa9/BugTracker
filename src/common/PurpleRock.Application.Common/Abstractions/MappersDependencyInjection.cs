using Microsoft.Extensions.DependencyInjection;
using PurpleRock.Application.Common.Abstractions.Implementations;

namespace PurpleRock.Application.Common.Abstractions;

/// <summary>
/// Provides extension methods for injecting Mappers.
/// </summary>
public static class MappersDependencyInjection
{
  /// <summary>
  /// Register Mappers Dependencies.
  /// </summary>
  /// <param name="services"></param>
  /// <param name="prAssemblies"></param>
  /// <returns></returns>
  public static IServiceCollection AddApplicationMappers(
      this IServiceCollection services,
      IEnumerable<Assembly> prAssemblies)
  {
    var mapperTypes = new[] { typeof(IModelMapper<,>), typeof(IExistingModelMapper<,>) };

    // Register auto mappers
    services.AddSingleton(typeof(IModelMapper<,>), typeof(MapsterModelMapper<,>));
    services.AddSingleton(typeof(IExistingModelMapper<,>), typeof(MapsterExistingModelMapper<,>));

    // Register manual/explicit mappers
    foreach (var assembly in prAssemblies)
    {
      foreach (var item in assembly.GetTypes())
      {
        var isConcrete = !item.IsAbstract && !item.IsInterface && !item.IsGenericType;
        if (isConcrete)
        {
          foreach (var mapperType in mapperTypes.Where(mapperType => IsItemMapperType(item, mapperType)))
          {
            var serviceType = item.GetInterfaces().First(i => i.GetGenericTypeDefinition() == mapperType);
            services.AddSingleton(serviceType, item);
          }
        }
      }
    }

    static bool IsItemMapperType(Type item, Type mapperType)
    {
      return item.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == mapperType);
    }

    return services;
  }
}