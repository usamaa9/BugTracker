using Microsoft.AspNetCore.Http;
using Swashbuckle.AspNetCore.Annotations;

namespace PurpleRock.WebApi.Common.Swagger.Attributes;

/// <summary>
/// A <see cref="SwaggerResponseAttribute"/> that denotes a 204 No Content response.
/// </summary>
public sealed class NoContentAttribute : SwaggerResponseAttribute
{
  /// <summary>
  /// Initializes a new instance of the <see cref="NoContentAttribute"/> class.
  /// </summary>
  /// <param name="description">The description to describe the attribute.</param>
  public NoContentAttribute(string? description = null)
      : base(StatusCodes.Status204NoContent, description)
  {
  }
}
