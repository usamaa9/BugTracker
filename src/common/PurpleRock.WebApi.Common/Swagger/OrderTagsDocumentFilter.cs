using Swashbuckle.AspNetCore.Annotations;

namespace PurpleRock.WebApi.Common.Swagger;

/// <summary>
/// Document filter used to sort the tags by the name in alphabetical order.
/// </summary>
internal class OrderTagsDocumentFilter : IDocumentFilter
{
  /// <inheritdoc/>
  public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
  {
    swaggerDoc.Tags = context.ApiDescriptions
      .SelectMany(x => x.ActionDescriptor.EndpointMetadata)
      .OfType<SwaggerOperationAttribute>()
      .SelectMany(x => x.Tags)
      .Distinct()
      .OrderBy(x => x)
      .Select(name => new OpenApiTag { Name = name })
      .ToList();
  }
}