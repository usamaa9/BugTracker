namespace PurpleRock.Application.Common.Configuration;

/// <summary>
/// Provides options for your application configuration.
/// </summary>
public class AppOptions : IConfigurationOptions
{
  /// <inheritdoc/>
  public string? SectionName => null;

  /// <summary>
  /// Application Name.
  /// </summary>
  [Required]
  public string ApplicationName { get; set; } = string.Empty;

  /// <summary>
  /// Application Display Name.
  /// </summary>
  [Required]
  public string ApplicationDisplayName { get; set; } = string.Empty;

  /// <summary>
  /// Allowed hosts.
  /// </summary>
  [Required]
  public string AllowedHosts { get; set; } = string.Empty;

  /// <summary>
  /// Flag to determine if the database and containers need to be created.
  /// </summary>
  [Required]
  public bool CreateDbAndContainers { get; set; }
}