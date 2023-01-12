using System.ComponentModel;

namespace PurpleRock.Application.Common.Extensions;

/// <summary>
/// The Type Extensions Class.
/// </summary>
public static class TypeExtensions
{
  /// <summary>
  /// Determines whether or not a type is Simple.
  /// </summary>
  /// <param name="type"></param>
  /// <returns></returns>
  public static bool IsSimple(this Type type)
  {
    return TypeDescriptor.GetConverter(type).CanConvertFrom(typeof(string));
  }
}
