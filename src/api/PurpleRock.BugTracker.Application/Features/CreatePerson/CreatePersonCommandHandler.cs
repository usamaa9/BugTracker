namespace PurpleRock.BugTracker.Application.Features.CreatePerson;

public class CreatePersonCommandHandler : IRequestHandler<CreatePersonCommand, Result<Unit>>
{
    private readonly ILogger<CreatePersonCommandHandler> _logger;
    private readonly IWritePersonRepository _personRepository;

    public CreatePersonCommandHandler(
        IWritePersonRepository personRepository,
        ILogger<CreatePersonCommandHandler> logger
    )
    {
        _personRepository = personRepository;
        _logger = logger;
    }

    public async Task<Result<Unit>> Handle(CreatePersonCommand request, CancellationToken cancellationToken)
    {
        var person = new Person
        {
            Id = Guid.NewGuid().ToString(),
            Name = request.Request.Name
        };

        var response = await _personRepository.CreateAsync(person);
        if (!response)
        {
            _logger.LogWarning("Failed to create person {personId}", person.Id);
            return Result.BadRequest<Unit>($"Failed to create person {person.Id}");
        }

        var location = request.LocationPath.Replace("{personId}", person.Id);

        return Result.Created(Unit.Value, new Uri(location, UriKind.Relative));
    }
}
