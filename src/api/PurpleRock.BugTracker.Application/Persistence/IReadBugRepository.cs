namespace PurpleRock.BugTracker.Application.Persistence;

/// <summary>
/// Repository for managing read operations on the bug store.
/// </summary>
public interface IReadBugRepository
{
    /// <summary>
    /// Returns a list of all bugs.
    /// </summary>
    /// <returns>A list of all <see cref="Bug"/> objects.</returns>
    Task<IReadOnlyCollection<Bug>> GetAllAsync();

    /// <summary>
    /// Retrieves a bug using its unique identifier.
    /// </summary>
    /// <param name="bugId">The id of the bug to retrieve.</param>
    /// <returns>A <see cref="Bug"/> object that represents the bug.</returns>
    /// <exception cref="NotFoundException"></exception>
    Task<Bug> GetAsync(string bugId);
}
