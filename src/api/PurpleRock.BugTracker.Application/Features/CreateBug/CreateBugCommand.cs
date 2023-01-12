using PurpleRock.BugTracker.Application.Contracts.Requests;

namespace PurpleRock.BugTracker.Application.Features.CreateBug;

/// <summary>
/// Command to create a bug.
/// <seealso cref="CreateBugCommandHandler"/>
/// </summary>
public class CreateBugCommand : IRequest<Result<Unit>>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CreateBugCommand"/> class.
    /// </summary>
    /// <param name="request"></param>
    /// <param name="locationPath"></param>
    public CreateBugCommand(CreateBugRequest request, string locationPath = "{bugId}")
    {
        Request = request;
        LocationPath = locationPath;
    }

    /// <summary>
    /// Request containing the bug creation details.
    /// </summary>
    public CreateBugRequest Request { get; }

    /// <summary>
    /// The path to the newly created bug.
    /// </summary>
    public string LocationPath { get; }
}
