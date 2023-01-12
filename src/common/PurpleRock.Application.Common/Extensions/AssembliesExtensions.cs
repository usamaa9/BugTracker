namespace PurpleRock.Application.Common.Extensions;

/// <summary>
/// Provides extension methods for the Assembly class.
/// </summary>
public static class AssembliesExtensions
{
  /// <summary>
  /// Allowed start names of assemblies.
  /// </summary>
  public static IReadOnlyCollection<string> AllowedAssemblyStartNames { get; } = new[] { "PurpleRock" };

  /// <summary>
  /// Abstraction of Assembly.Load().
  /// </summary>
  public static Func<AssemblyName, Assembly> AssemblyLoad { get; set; } = assemblyName => Assembly.Load(assemblyName);

  /// <summary>
  /// Method to retrieve PurpleRock assemblies.
  /// </summary>
  /// <param name="entryAssembly"></param>
  /// <returns></returns>
  public static Assembly[] GetPurpleRockAssemblies(this Assembly entryAssembly)
  {
    var prAssemblies = GetAssemblies(
        entryAssembly,
        assembly =>
        {
          return AllowedAssemblyStartNames
                  .Any(part => assembly.FullName.StartsWith(part, StringComparison.OrdinalIgnoreCase));
        });

    return prAssemblies;
  }

  private static Assembly[] GetAssemblies(Assembly entryAssembly, Func<AssemblyName, bool>? filter = null)
  {
    _ = entryAssembly ?? throw new ArgumentNullException(nameof(entryAssembly));

    var returnAssemblies = new List<Assembly>();
    if (filter is not null && filter(entryAssembly.GetName()))
    {
      returnAssemblies.Add(entryAssembly);
    }

    var loadedAssemblies = new HashSet<string>();
    var assembliesToCheck = new Queue<Assembly>();
    assembliesToCheck.Enqueue(entryAssembly);

    while (assembliesToCheck.Any())
    {
      var assemblyToCheck = assembliesToCheck.Dequeue();
      foreach (var reference in assemblyToCheck.GetReferencedAssemblies().Where(filter ?? (_ => true)))
      {
        if (loadedAssemblies.Contains(reference.FullName))
        {
          continue;
        }

        var assembly = AssemblyLoad(reference);
        assembliesToCheck.Enqueue(assembly);
        loadedAssemblies.Add(reference.FullName);
        returnAssemblies.Add(assembly);
      }
    }

    return returnAssemblies.ToArray();
  }
}