namespace PurpleRock.Application.Common.Configuration;

/// <summary>
/// Provides options for connection strings.
/// </summary>
public class ConnectionStringsOptions : IConfigurationOptions
{
  /// <inheritdoc />
  public string SectionName => "ConnectionStrings";

  /// <summary>
  /// Url to the App Configuration Store.
  /// </summary>
  public string AppConfigurationUri { get; set; } = string.Empty;

  /// <summary>
  /// Azure event Hub connection string.
  /// </summary>
  public string AzureEventHub { get; set; } = string.Empty;
}
