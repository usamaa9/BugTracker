namespace PurpleRock.BugTracker.Application.Features.AssignBug;

/// <summary>
/// Assign bug to person.
/// <seealso cref="AssignBugCommandHandler"/>
/// </summary>
public class AssignBugCommand : IRequest<Result<Unit>>
{
    public AssignBugCommand(string bugId, string personId)
    {
        BugId = bugId;
        PersonId = personId;
    }

    public string PersonId { get; set; }


    public string BugId { get; set; }
}
