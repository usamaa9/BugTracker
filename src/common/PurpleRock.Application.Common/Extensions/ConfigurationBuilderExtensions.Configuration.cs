using Azure.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.AzureAppConfiguration;
using Microsoft.Extensions.Configuration.EnvironmentVariables;
using Microsoft.Extensions.Configuration.Json;
using PurpleRock.Application.Common.Configuration;

namespace PurpleRock.Application.Common.Extensions;

/// <summary>
/// Extensions to handle IConfiguration.
/// </summary>
[ExcludeFromCodeCoverage]
public static class ConfigurationBuilderExtensions
{
  /// <summary>
  /// The Azure Token Credential.
  /// </summary>
  private static DefaultAzureCredential TokenCredential =>
      new(new DefaultAzureCredentialOptions
      {
        // Exclude shared token cache to stop problem in docker linux
        // https://github.com/Azure/azure-sdk-for-net/issues/17052
        // https://github.com/Azure/azure-sdk/issues/1970
        ExcludeSharedTokenCacheCredential = true
      });

  /// <summary>
  /// Configures the Functions builder with the configuration settings.
  /// </summary>
  /// <param name="builder"></param>
  /// <param name="environmentName"></param>
  /// <returns></returns>
  public static IConfiguration LoadPurpleRockConfiguration(
      this IConfigurationBuilder builder,
      string environmentName)
  {
    var configuration = builder.LoadDefaultAndEnvironmentVariables(environmentName);

    var appOptions = configuration.Get<AppOptions>();

    // Re-add the environment JSON provider so it takes precedence over azure config
    var envJson = builder.Sources.SingleOrDefault(x =>
        (x as JsonConfigurationSource)?.Path == $"appsettings.{environmentName}.json");

    if (envJson != null)
    {
      builder.Sources.Remove(envJson);
      builder.Sources.Add(envJson);
    }

    return builder.Build();
  }

  /// <summary>
  /// Returns a configuration object having the default appsettings
  /// and environmental variables loaded.
  /// </summary>
  /// <param name="builder"></param>
  /// <param name="environmentName"></param>
  /// <returns></returns>
  public static IConfiguration LoadDefaultAndEnvironmentVariables(
      this IConfigurationBuilder builder,
      string? environmentName = null)
  {
    builder.AddJsonFileIfNotThere("appsettings.json", false);

    // So that function name/declaration/definition values can be resolved
    // https://stackoverflow.com/a/65418048
    builder.AddJsonFileIfNotThere("local.settings.json", true);
    if (environmentName is not null)
    {
      builder.AddJsonFileIfNotThere($"appsettings.{environmentName}.json", true);
    }

    if (!builder.Sources.Any(x => x is EnvironmentVariablesConfigurationSource))
    {
      builder.AddEnvironmentVariables();
    }

    return builder.Build();
  }

  private static void AddJsonFileIfNotThere(this IConfigurationBuilder builder, string path, bool optional)
  {
    if (builder.Sources.Any(x => (x as JsonConfigurationSource)?.Path == path))
    {
      return;
    }

    builder.AddJsonFile(path, optional);
  }
}