namespace PurpleRock.BugTracker.Application.Features.GetAllBugs;

/// <summary>
/// Query to retrieve list of all bugs.
/// <see cref="GetAllBugsQueryHandler"/>
/// </summary>
public class GetAllBugsQuery : IRequest<Result<IReadOnlyCollection<BugDetailsResponse>>>
{
    public string? Status { get; set; }
}
