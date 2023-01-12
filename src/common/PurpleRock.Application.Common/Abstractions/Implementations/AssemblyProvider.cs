namespace PurpleRock.Application.Common.Abstractions.Implementations;

/// <inheritdoc cref="IAssemblyProvider"/>
[ExcludeFromCodeCoverage]
internal class AssemblyProvider : IAssemblyProvider
{
  /// <inheritdoc/>
  public Assembly GetEntryAssembly()
  {
    // Returns the process executable. Keep in mind that this may not be your executable.
    var assembly = Assembly.GetEntryAssembly();

    return
        assembly ??
        throw new InvalidOperationException("Could not retrieve Entry Assembly");
  }
}
