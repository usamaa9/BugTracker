using Microsoft.AspNetCore.Http;
using Swashbuckle.AspNetCore.Annotations;

namespace PurpleRock.WebApi.Common.Swagger.Attributes;

/// <summary>
/// A <see cref="SwaggerResponseAttribute"/> that denotes a 500 Internal Server error response.
/// </summary>
public sealed class InternalServerErrorAttribute : SwaggerResponseAttribute
{
  /// <summary>
  /// Initializes a new instance of the <see cref="InternalServerErrorAttribute"/> class.
  /// </summary>
  public InternalServerErrorAttribute()
      : base(StatusCodes.Status500InternalServerError, "Server Error")
  {
  }
}
