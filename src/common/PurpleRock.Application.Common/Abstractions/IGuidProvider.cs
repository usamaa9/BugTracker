namespace PurpleRock.Application.Common.Abstractions;

/// <summary>
/// Provides abstraction for the Guid static methods.
/// </summary>
public interface IGuidProvider
{
  /// <summary>
  /// Abstraction for Guid.NewGuid().
  /// </summary>
  Guid NewGuid { get; }
}
