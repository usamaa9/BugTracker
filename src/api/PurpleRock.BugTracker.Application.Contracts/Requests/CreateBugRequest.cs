namespace PurpleRock.BugTracker.Application.Contracts.Requests;

/// <summary>
/// Create Bug request.
/// </summary>
[ExcludeFromCodeCoverage]
public record CreateBugRequest : IExampleProvider<CreateBugRequest>
{
    /// <summary>
    /// Title of the bug.
    /// </summary>
    public string? Title { get; init; }

    /// <summary>
    /// Description of the bug.
    /// </summary>
    public string? Description { get; init; }

    public CreateBugRequest Generate()
    {
        return new Faker<CreateBugRequest>()
            .RuleFor(bp => bp.Title, f => f.Lorem.Sentence(5))
            .RuleFor(bp => bp.Description, f => f.Lorem.Sentence(12));
    }
}
