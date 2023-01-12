using Microsoft.AspNetCore.Http;
using Swashbuckle.AspNetCore.Annotations;

namespace PurpleRock.WebApi.Common.Swagger.Attributes;

/// <summary>
/// A <see cref="SwaggerResponseAttribute"/> that denotes a 404 Not Found response.
/// </summary>
public sealed class NotFoundAttribute : SwaggerResponseAttribute
{
  /// <summary>
  /// Initializes a new instance of the <see cref="NotFoundAttribute"/> class.
  /// </summary>
  /// <param name="entityName">The name of the entity that should have been retrieved.</param>
  public NotFoundAttribute(string entityName)
  : base(StatusCodes.Status404NotFound)
  {
    EntityName = entityName;
    Description = $"{entityName} not found";
  }

  /// <summary>
  /// The name of the entity that should have been retrieved.
  /// </summary>
  public string EntityName { get; }
}
