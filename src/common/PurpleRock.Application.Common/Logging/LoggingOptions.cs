using PurpleRock.Application.Common.Configuration;

namespace PurpleRock.Application.Common.Logging;

/// <summary>
/// Provides options for logging.
/// </summary>
public class LoggingOptions : IConfigurationOptions
{
  /// <inheritdoc/>
  public string SectionName => "Logging";

  /// <summary>
  /// Level of logging.
  /// </summary>
  [Required]
  public Dictionary<string, string> LogLevel { get; set; } = null!;
}