#pragma warning disable IDE0072 // Add missing cases

namespace PurpleRock.WebApi.Common.Extensions;

/// <summary>
/// Extension helpers for Result objects.
/// </summary>
public static class ResultExtensions
{
    /// <summary>
    /// Map Results to an <see cref="IActionResult"/> based on the status code.
    /// </summary>
    /// <typeparam name="T">The inner type of the result.</typeparam>
    /// <param name="result">The result object to map.</param>
    /// <returns></returns>
    public static IActionResult ToActionResult<T>(this Result<T> result)
    {
        return result.Status switch
        {
            HttpStatusCode.OK
                => new OkObjectResult(result.Value),

            HttpStatusCode.Created
                => new CreatedResult(
                    result.CustomState == null
                        ? string.Empty
                        : result.CustomState!.ToString() ?? string.Empty,
                    default),

            HttpStatusCode.NoContent
                => new NoContentResult(),

            HttpStatusCode.NotFound
                => result.CustomState == null
                    ? new NotFoundResult()
                    : new NotFoundObjectResult(result.CustomState),

            HttpStatusCode.BadRequest
                => result.CustomState == null
                    ? new BadRequestResult()
                    : new BadRequestObjectResult(result.CustomState),

            HttpStatusCode.InternalServerError
                => new ObjectResult(result.CustomState)
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError
                },

            _ => new ObjectResult(result.CustomState)
            {
                StatusCode = (int)result.Status
            }
        };
    }
}
