using System.Text.Json.Serialization;

namespace PurpleRock.WebApi.Common.Converters;

/// <summary>
/// <see cref="JsonConverter"/> for strings that remove all leading and trailing whitespace when deserialising.
/// </summary>
public class JsonStringTrimConverter : JsonConverter<string?>
{
  /// <inheritdoc/>
  public override string? Read(
      ref Utf8JsonReader reader,
      Type typeToConvert,
      JsonSerializerOptions options)
  {
    return reader.GetString()?.Trim();
  }

  /// <inheritdoc/>
  public override void Write(
      Utf8JsonWriter writer,
      string? value,
      JsonSerializerOptions options)
  {
    writer.WriteStringValue(value);
  }
}