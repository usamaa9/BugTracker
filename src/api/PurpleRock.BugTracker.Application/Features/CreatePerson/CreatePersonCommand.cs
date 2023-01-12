using PurpleRock.BugTracker.Application.Contracts.Requests;

namespace PurpleRock.BugTracker.Application.Features.CreatePerson;

/// <summary>
/// Command to add a person.
/// <seealso cref="CreatePersonCommandHandler"/>
/// </summary>
public class CreatePersonCommand : IRequest<Result<Unit>>
{
    public CreatePersonCommand(CreatePersonRequest request, string locationPath = "{personId}")
    {
        Request = request;
        LocationPath = locationPath;
    }

    /// <summary>
    /// Request containing the person creation details.
    /// </summary>
    public CreatePersonRequest Request { get; }

    /// <summary>
    /// The path to the newly created person.
    /// </summary>
    public string LocationPath { get; }
}
