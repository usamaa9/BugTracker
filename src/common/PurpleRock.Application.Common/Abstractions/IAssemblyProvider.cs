namespace PurpleRock.Application.Common.Abstractions;

/// <summary>
/// Retrieve assemblies.
/// </summary>
public interface IAssemblyProvider
{
  /// <summary>
  /// Gets the process executable in the default application domain. 
  /// </summary>
  /// <returns></returns>
  Assembly GetEntryAssembly();
}
