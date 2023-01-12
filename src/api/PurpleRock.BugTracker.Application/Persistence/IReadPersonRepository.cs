namespace PurpleRock.BugTracker.Application.Persistence;

/// <summary>
/// Repository for managing read operations on the person store.
/// </summary>
public interface IReadPersonRepository
{
    /// <summary>
    /// Retrieves a person using its unique identifier.
    /// </summary>
    /// <param name="personId">The id of the person to retrieve.</param>
    /// <returns>A <see cref="Person"/> object that represents the person.</returns>
    /// <exception cref="NotFoundException"></exception>
    Task<Person?> GetAsync(string personId);
}
