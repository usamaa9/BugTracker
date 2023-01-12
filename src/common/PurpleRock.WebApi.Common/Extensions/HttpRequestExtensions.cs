namespace PurpleRock.WebApi.Common.Extensions;

/// <summary>
/// Extension methods for <see cref="HttpRequest"/>.
/// </summary>
public static class HttpRequestExtensions
{
    /// <summary>
    /// Deserialises the body of the <see cref="HttpRequest"/> to the requested type.
    /// </summary>
    /// <typeparam name="TValue">The type that the body should be deserialised into.</typeparam>
    /// <param name="request">The HttpRequest with a body to convert.</param>
    /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
    public static async Task<TValue> BodyToAsync<TValue>(this HttpRequest request)
    {
        request.Body.Position = 0;
        var value = await JsonSerializer.DeserializeAsync<TValue>(
            request.Body,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        return value!;
    }
}
