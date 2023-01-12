namespace PurpleRock.BugTracker.Application.Contracts.Responses;

/// <summary>
/// Person Details
/// </summary>
[ExcludeFromCodeCoverage]
public class PersonResponse : IExampleProvider<PersonResponse>
{
    /// <summary>
    /// Person's unique identifier.
    /// </summary>
    public string? Id { get; set; }

    /// <summary>
    /// Name of person.
    /// </summary>
    public string? Name { get; set; }

    public PersonResponse Generate()
    {
        return new Faker<PersonResponse>()
            .RuleFor(bp => bp.Id, f => f.Random.Guid().ToString())
            .RuleFor(bp => bp.Name, f => f.Person.FullName)
            .Generate();
    }
}
