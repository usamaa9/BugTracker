namespace PurpleRock.Application.Common.Commands;

/// <summary>
/// Dictionary to hold data across behaviors.
/// </summary>

public class CommandItemsCache : Dictionary<string, object>
{
  /// <summary>
  /// Key for the context value.
  /// </summary>
  public const string CtxKey = "_ctx";

  /// <summary>
  /// Adds object to context.
  /// </summary>
  /// <param name="context"></param>
  /// <exception cref="InvalidOperationException">When the context already exists.</exception>
  public void AddContext(object context)
  {
    if (ContainsKey(CtxKey))
    {
      throw new InvalidOperationException("Context has already been provided");
    }

    Add(CtxKey, context);
  }

  /// <summary>
  /// Gets the value stored by the context.
  /// </summary>
  /// <returns></returns>
  public object? GetContext()
  {
    if (TryGetValue(CtxKey, out var ctxValue))
    {
      return ctxValue;
    }

    return null;
  }
}