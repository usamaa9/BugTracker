namespace PurpleRock.BugTracker.Application.Features.CreateBug;

public class CreateBugCommandHandler : IRequestHandler<CreateBugCommand, Result<Unit>>
{
    private readonly ILogger<CreateBugCommandHandler> _logger;
    private readonly IWriteBugRepository _writeBugRepository;

    public CreateBugCommandHandler(
        IWriteBugRepository writeBugRepository,
        ILogger<CreateBugCommandHandler> logger
    )
    {
        _writeBugRepository = writeBugRepository;
        _logger = logger;
    }

    public async Task<Result<Unit>> Handle(CreateBugCommand request, CancellationToken cancellationToken)
    {
        var bug = new Bug
        {
            Id = Guid.NewGuid().ToString(),
            Title = request.Request.Title,
            Description = request.Request.Description,
            DateOpened = DateTime.Now,
            DateClosed = null,
            AssignedTo = null
        };
        var response = await _writeBugRepository.CreateAsync(bug);

        if (!response)
        {
            _logger.LogWarning("Failed to create bug {bugId}", bug.Id);
            return Result.BadRequest<Unit>($"Failed to create bug {bug.Id}");
        }

        var location = request.LocationPath.Replace("{bugId}", bug.Id);

        return Result.Created(Unit.Value, new Uri(location, UriKind.Relative));
    }
}
