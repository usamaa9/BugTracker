namespace PurpleRock.BugTracker.Application.Persistence;

/// <summary>
/// Repository for managing write operations on the bug store.
/// </summary>
public interface IWriteBugRepository
{
    /// <summary>
    /// Stores a new bug.
    /// </summary>
    /// <param name="bug">The bug to store.</param>
    /// <returns>True if the bug was created successfully.</returns>
    public Task<bool> CreateAsync(Bug bug);

    /// <summary>
    /// Updates an existing bug.
    /// </summary>
    /// <param name="bug">The bug to update/</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public Task UpdateAsync(Bug bug);
}
