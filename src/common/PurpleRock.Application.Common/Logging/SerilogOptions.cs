using PurpleRock.Application.Common.Configuration;

namespace PurpleRock.Application.Common.Logging;

/// <summary>
/// Provides options for Serilog configuration.
/// </summary>
public class SerilogOptions : IConfigurationOptions
{
  /// <inheritdoc/>
  public string SectionName => "Serilog";

  /// <summary>
  /// Places to publish logs.
  /// </summary>
  [Required]
  public List<string> Using { get; set; } = null!;

  /// <summary>
  /// Minimum level of logging.
  /// </summary>
  [Required]
  public string MinimumLevel { get; set; } = null!;

  /// <summary>
  /// Settings for the specific logging places.
  /// </summary>
  [Required]
  public List<Dictionary<string, string>> WriteTo { get; set; } = null!;
}