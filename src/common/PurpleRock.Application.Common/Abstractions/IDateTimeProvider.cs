namespace PurpleRock.Application.Common.Abstractions;

/// <summary>
/// Retrieve DateTime information.
/// </summary>
public interface IDateTimeProvider
{
  /// <summary>
  /// Get the DateTime as it is now.
  /// </summary>
  DateTime Now => DateTime.UtcNow;
}
