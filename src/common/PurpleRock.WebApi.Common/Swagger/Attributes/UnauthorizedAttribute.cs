using Microsoft.AspNetCore.Http;
using Swashbuckle.AspNetCore.Annotations;

namespace PurpleRock.WebApi.Common.Swagger.Attributes;

/// <summary>
/// A <see cref="SwaggerResponseAttribute"/> that denotes a 401 Unauthorized response.
/// </summary>
public sealed class UnauthorizedAttribute : SwaggerResponseAttribute
{
  /// <summary>
  /// Initializes a new instance of the <see cref="UnauthorizedAttribute"/> class.
  /// </summary>
  public UnauthorizedAttribute()
      : base(StatusCodes.Status401Unauthorized, "Unauthorized")
  {
  }
}
