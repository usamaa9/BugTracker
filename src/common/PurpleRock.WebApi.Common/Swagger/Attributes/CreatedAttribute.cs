using Microsoft.AspNetCore.Http;
using Swashbuckle.AspNetCore.Annotations;

namespace PurpleRock.WebApi.Common.Swagger.Attributes;

/// <summary>
/// A <see cref="SwaggerResponseAttribute"/> that denotes a 201 Created response.
/// </summary>
public sealed class CreatedAttribute : SwaggerResponseAttribute
{
  /// <summary>
  /// Initializes a new instance of the <see cref="CreatedAttribute"/> class.
  /// </summary>
  /// <param name="description">The description to describe the attribute.</param>
  /// <param name="type">The type of the response body.</param>
  public CreatedAttribute(string description = "Created", Type? type = null)
  : base(StatusCodes.Status201Created, description, type, "application/json")
  {
  }
}
