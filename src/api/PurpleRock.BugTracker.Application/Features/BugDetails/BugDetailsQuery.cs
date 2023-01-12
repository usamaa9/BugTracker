namespace PurpleRock.BugTracker.Application.Features.BugDetails;

/// <summary>
/// Retrieves bug details.
/// <see cref="BugDetailsQueryHandler"/>
/// </summary>
public class BugDetailsQuery : IRequest<Result<BugDetailsResponse>>
{
    public BugDetailsQuery(string bugId)
    {
        BugId = bugId;
    }

    /// <summary>
    /// The unique bug identifier.
    /// </summary>
    public string BugId { get; set; }
}
