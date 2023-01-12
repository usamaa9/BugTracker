namespace PurpleRock.WebApi.Common.Extensions;

/// <summary>
/// Command Bus Extensions specific to WebApi.
/// </summary>
public static class CommandBusExtensions
{
    /// <summary>
    /// Send a command and returns the corresponding response.
    /// </summary>
    /// <typeparam name="TCommand">The type of the command that is being sent.</typeparam>
    /// <typeparam name="T">The type of the response that should be received.</typeparam>
    /// <param name="commandBus">The command bus used to send and receive messages.</param>
    /// <param name="ctx">The context which is used to generate the command.</param>
    /// <returns>The response.</returns>
    public static async Task<Result<T>> SendAsync<TCommand, T>(
        this ICommandBus commandBus, HttpContext ctx)
        where TCommand : IRequest<Result<T>>
    {
        var command = await ctx.Request.BodyToAsync<TCommand>();

        return await commandBus.SendAsync<TCommand, T>(ctx, command);
    }

    /// <summary>
    /// Send a command and returns the corresponding response.
    /// </summary>
    /// <typeparam name="TCommand">The type of the command that is being sent.</typeparam>
    /// <typeparam name="T">The type of the response that should be received.</typeparam>
    /// <param name="commandBus">The command bus used to send and receive messages.</param>
    /// <param name="ctx">The context which is used to generate the command metadata.</param>
    /// <param name="command">The command to send.</param>
    /// <returns>The response.</returns>
    public static async Task<Result<T>> SendAsync<TCommand, T>(
        this ICommandBus commandBus, HttpContext ctx, TCommand command)
        where TCommand : IRequest<Result<T>>
    {
        var commandMetadata = new CommandMetadata(DateTime.UtcNow, Guid.NewGuid().ToString(), ctx);
        var result = await commandBus.SendAsync<TCommand, T>(command, commandMetadata);
        return result;
    }
}
