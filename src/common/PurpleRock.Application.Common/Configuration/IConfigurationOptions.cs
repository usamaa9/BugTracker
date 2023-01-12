namespace PurpleRock.Application.Common.Configuration;

/// <summary>
/// Provides abstraction for configuration options.
/// </summary>
public interface IConfigurationOptions
{
  /// <summary>
  /// Name of a configuration section.
  /// </summary>
  public string? SectionName { get; }
}