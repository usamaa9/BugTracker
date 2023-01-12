using Microsoft.Extensions.Logging;

namespace PurpleRock.WebApi.Common.Extensions;

/// <summary>
/// Service collection middleware extensions.
/// </summary>
public static partial class ServiceCollectionExtensions
{
    /// <summary>
    /// Update Model State middleware to log invalid response reason.
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection ConfigureInvalidModelStateMiddleware(this IServiceCollection services)
    {
        services.PostConfigure<ApiBehaviorOptions>(options =>
        {
            var builtInFactory = options.InvalidModelStateResponseFactory;

            options.InvalidModelStateResponseFactory = context =>
            {
                var loggerFactory = context.HttpContext.RequestServices
                    .GetRequiredService<ILoggerFactory>();
                var logger = loggerFactory.CreateLogger(context.ActionDescriptor.DisplayName!);

                var serializableModelState = new SerializableError(context.ModelState);

                var jsonMessage = JsonSerializer.Serialize(serializableModelState);

                logger.LogInformation("Invalid Model State:{JsonMessage}", jsonMessage);

                return builtInFactory(context);
            };
        });

        return services;
    }
}
