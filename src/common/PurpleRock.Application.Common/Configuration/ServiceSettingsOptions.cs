namespace PurpleRock.Application.Common.Configuration;

/// <summary>
/// Provides options for service settings.
/// </summary>
public class ServiceSettingsOptions : IConfigurationOptions
{
  /// <inheritdoc />
  public string SectionName => "ServiceSettings";

  /// <summary>
  /// The type of service e.g. Api.
  /// </summary>
  public string ServiceType { get; set; } = string.Empty;
}