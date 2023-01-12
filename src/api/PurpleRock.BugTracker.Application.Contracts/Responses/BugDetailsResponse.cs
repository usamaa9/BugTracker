namespace PurpleRock.BugTracker.Application.Contracts.Responses;

public class BugDetailsResponse : IExampleProvider<BugDetailsResponse>
{
    /// <summary>
    /// Unique identifier of the bug.
    /// </summary>
    public string? Id { get; set; }

    /// <summary>
    /// Title of the bug.
    /// </summary>
    public string? Title { get; set; }

    /// <summary>
    /// Description of the bug.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Date the bug was opened.
    /// </summary>
    public DateTime? DateOpened { get; set; }

    /// <summary>
    /// Date the bug was closed
    /// </summary>
    public DateTime? DateClosed { get; set; }

    /// <summary>
    /// True if the bug is open, false if closed.
    /// </summary>
    public bool IsOpen => DateClosed == null;

    /// <summary>
    /// The person the bug is assigned to.
    /// </summary>
    public PersonResponse? AssignedTo { get; set; }

    public BugDetailsResponse Generate()
    {
        return new Faker<BugDetailsResponse>()
            .RuleFor(bp => bp.Id, f => f.Random.Guid().ToString())
            .RuleFor(bp => bp.Title, f => f.Lorem.Sentence(5))
            .RuleFor(bp => bp.Description, f => f.Lorem.Sentence(13))
            .RuleFor(bp => bp.DateOpened, f => f.Date.Past())
            .RuleFor(bp => bp.DateClosed, f => f.Date.Future())
            .RuleFor(bp => bp.AssignedTo, new PersonResponse().Generate)
            .Generate();
    }
}
