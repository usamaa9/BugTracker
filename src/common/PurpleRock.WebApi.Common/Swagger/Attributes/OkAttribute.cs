using Microsoft.AspNetCore.Http;
using Swashbuckle.AspNetCore.Annotations;

namespace PurpleRock.WebApi.Common.Swagger.Attributes;

/// <summary>
/// A <see cref="SwaggerResponseAttribute"/> that denotes a 200 OK response.
/// </summary>
public sealed class OkAttribute : SwaggerResponseAttribute
{
  /// <summary>
  /// Initializes a new instance of the <see cref="OkAttribute"/> class.
  /// </summary>
  /// <param name="description">The description to describe the attribute.</param>
  /// <param name="type">The type returned in the response body.</param>
  public OkAttribute(string? description = null, Type? type = null)
      : base(StatusCodes.Status200OK, description, type, "application/json")
  {
  }
}
