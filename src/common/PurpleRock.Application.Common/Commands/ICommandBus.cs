namespace PurpleRock.Application.Common.Commands;

/// <summary>
/// Command Bus used to send and receive messages.
/// </summary>
public interface ICommandBus
{
  /// <summary>
  /// Send a command and returns the corresponding response.
  /// </summary>
  /// <typeparam name="TCommand">The type of the command that is being sent.</typeparam>
  /// <typeparam name="T">The type of the response that should be received.</typeparam>
  /// <param name="command">The command that is being sent.</param>
  /// <param name="metadata">The command's metadata.</param>
  /// <returns>The response.</returns>
  Task<dynamic> SendAsync<TCommand, T>(TCommand command, CommandMetadata metadata)
    where TCommand : IRequest<Result<T>>;
}