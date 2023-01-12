namespace PurpleRock.BugTracker.Application.Features.CloseBug;

/// <summary>
/// Command to close a bug.
/// <seealso cref="CloseBugCommandHandler"/>
/// </summary>
public class CloseBugCommand : IRequest<Result<Unit>>
{
    public CloseBugCommand(string bugId)
    {
        BugId = bugId;
    }

    public string BugId { get; set; }
}
