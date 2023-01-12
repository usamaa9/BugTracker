namespace PurpleRock.BugTracker.Application.Contracts.Requests;

/// <summary>
/// Request for adding a person.
/// </summary>
[ExcludeFromCodeCoverage]
public record CreatePersonRequest : IExampleProvider<CreatePersonRequest>
{
    /// <summary>
    /// Name of the person.
    /// </summary>
    public string? Name { get; init; }

    public CreatePersonRequest Generate()
    {
        return new Faker<CreatePersonRequest>()
            .RuleFor(bp => bp.Name, f => f.Person.FullName);
    }
}
