using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace PurpleRock.WebApi.Common.Swagger.Attributes;

/// <summary>
/// A <see cref="SwaggerResponseAttribute"/> that denotes a 400 Bad Request response.
/// </summary>
public sealed class BadRequestAttribute : SwaggerResponseAttribute
{
  /// <summary>
  /// Initializes a new instance of the <see cref="BadRequestAttribute"/> class.
  /// </summary>
  /// <param name="description">The description to describe the attribute.</param>
  public BadRequestAttribute(string description = "Bad Request")
  : base(StatusCodes.Status400BadRequest, description, typeof(ValidationProblemDetails))
  {
  }
}
