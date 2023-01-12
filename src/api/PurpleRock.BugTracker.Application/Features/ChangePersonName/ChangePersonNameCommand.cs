namespace PurpleRock.BugTracker.Application.Features.ChangePersonName;

/// <summary>
/// Command to change a person's name.
/// <seealso cref="ChangePersonNameCommandHandler"/>
/// </summary>
public class ChangePersonNameCommand : IRequest<Result<Unit>>
{
    public ChangePersonNameCommand(string? personId, string? name)
    {
        PersonId = personId;
        Name = name;
    }

    public string? PersonId { get; set; }
    public string? Name { get; set; }
}
