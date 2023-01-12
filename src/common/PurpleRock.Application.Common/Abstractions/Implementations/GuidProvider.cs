namespace PurpleRock.Application.Common.Abstractions.Implementations;

/// <inheritdoc cref="IGuidProvider"/>
internal class GuidProvider : IGuidProvider
{
  /// <inheritdoc/>
  public Guid NewGuid => Guid.NewGuid();
}
