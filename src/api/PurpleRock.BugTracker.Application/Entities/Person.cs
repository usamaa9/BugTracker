using Bogus;

namespace PurpleRock.BugTracker.Application.Entities;

public class Person : IExampleProvider<Person>
{
    /// <summary>
    /// Person's unique identifier.
    /// </summary>
    public string? Id { get; set; }

    /// <summary>
    /// Name of person.
    /// </summary>
    public string? Name { get; set; }

    [ExcludeFromCodeCoverage]
    public Person Generate()
    {
        return new Faker<Person>()
            .RuleFor(bp => bp.Id, f => f.Random.Guid().ToString())
            .RuleFor(bp => bp.Name, f => f.Person.FullName)
            .Generate();
    }
}
