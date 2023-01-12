namespace PurpleRock.BugTracker.Application.Persistence;

/// <summary>
///  Repository for managing write operations on the person store.
/// </summary>
public interface IWritePersonRepository
{
    /// <summary>
    /// Stores a new person.
    /// </summary>
    /// <param name="person">Person to store</param>
    /// <returns>True if the person was created successfully, false otherwise.</returns>
    public Task<bool> CreateAsync(Person person);

    /// <summary>
    /// Updates an existing person record.
    /// </summary>
    /// <param name="person"></param>
    /// <returns></returns>
    Task UpdateAsync(Person person);
}
