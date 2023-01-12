using PurpleRock.Application.Common.Configuration;
using PurpleRock.WebApi.Common.Swagger;
using Swashbuckle.AspNetCore.Filters;

namespace PurpleRock.WebApi.Common.Extensions;

/// <content>
/// Extension methods for <see cref="IServiceCollection"/> relating to Swagger generation.
/// </content>
public static partial class ServiceCollectionExtensions
{
  /// <summary>
  /// Setup generation of the swagger page.
  /// </summary>
  /// <param name="services"></param>
  /// <param name="appOptions"></param>
  /// <param name="prAssemblies">Assemblies specific to Purple Rock.</param>
  /// <returns></returns>
  public static IServiceCollection ConfigureSwagger(
    this IServiceCollection services,
    AppOptions appOptions,
    Assembly[] prAssemblies)
  {
    services.AddSwaggerExamplesFromAssemblies(prAssemblies);
    services.AddSwaggerExamplesFromAssemblies(SwaggerExamplesGenerator.Generate(appOptions, prAssemblies));
    services.AddSwaggerGen(c =>
    {
      c.EnableAnnotations();
      c.SwaggerDoc("v1", new OpenApiInfo
      {
        Version = "v1",
        Title = $"{appOptions.ApplicationDisplayName} V1"
      });

      c.IgnoreObsoleteActions();

      // Example filters when required
      c.ExampleFilters();

      // Include XML comments for the generated file from Models project
      var dir = new DirectoryInfo(AppContext.BaseDirectory);
      var files = dir.GetFiles("*.xml", SearchOption.TopDirectoryOnly);
      foreach (var file in files)
      {
        c.IncludeXmlComments(file.ToString());

        // apply schema filter to add description of enum members.
        c.SchemaFilter<DescribeEnumMembers>(file);
      }

      c.DocumentFilter<OrderTagsDocumentFilter>();
    });

    return services;
  }
}