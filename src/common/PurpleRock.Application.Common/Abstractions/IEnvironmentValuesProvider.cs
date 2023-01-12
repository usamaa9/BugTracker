namespace PurpleRock.Application.Common.Abstractions;

/// <summary>
/// Retrieve Environment Values.
/// </summary>
public interface IEnvironmentValuesProvider
{
  /// <inheritdoc cref="Environment.GetEnvironmentVariable(string)"/>
  string? GetEnvironmentVariable(string variable)
  {
    return Environment.GetEnvironmentVariable(variable);
  }
}
