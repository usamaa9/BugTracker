namespace PurpleRock.Application.Common.Commands;

/// <summary>
/// Class to hold a commands metadata.
/// </summary>
public class CommandMetadata
{
  /// <summary>
  /// Initializes a new instance of the <see cref="CommandMetadata"/> class.
  /// </summary>
  /// <param name="timestamp"></param>
  /// <param name="correlationId"></param>
  /// <param name="context"></param>
  /// <exception cref="ArgumentNullException"> When the correlation id is null or empty.</exception>
  public CommandMetadata(
      DateTime timestamp,
      string correlationId,
      dynamic? context = null)
  {
    if (string.IsNullOrWhiteSpace(correlationId))
    {
      throw new ArgumentNullException(nameof(correlationId));
    }

    Timestamp = timestamp;
    CorrelationId = correlationId;
    Context = context;
  }

  /// <summary>
  /// Gets the timestamp.
  /// </summary>
  public DateTime Timestamp { get; }

  /// <summary>
  /// Gets the correlation identifier.
  /// </summary>
  public string CorrelationId { get; }

  /// <summary>
  /// Gets the custom context object.
  /// </summary>
  public dynamic? Context { get; }
}
