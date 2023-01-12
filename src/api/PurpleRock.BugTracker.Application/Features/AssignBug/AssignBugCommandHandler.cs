namespace PurpleRock.BugTracker.Application.Features.AssignBug;

public class AssignBugCommandHandler : IRequestHandler<AssignBugCommand, Result<Unit>>
{
    private readonly ILogger<AssignBugCommandHandler> _logger;
    private readonly IReadBugRepository _readBugRepository;
    private readonly IReadPersonRepository _readPersonRepository;
    private readonly IWriteBugRepository _writeBugRepository;

    public AssignBugCommandHandler(
        IReadBugRepository readBugRepository,
        IWriteBugRepository writeBugRepository,
        IReadPersonRepository readPersonRepository,
        ILogger<AssignBugCommandHandler> logger
    )
    {
        _readBugRepository = readBugRepository;
        _writeBugRepository = writeBugRepository;
        _readPersonRepository = readPersonRepository;
        _logger = logger;
    }

    public async Task<Result<Unit>> Handle(AssignBugCommand request, CancellationToken cancellationToken)
    {
        var bugId = request.BugId;

        var existingBug = await _readBugRepository.GetAsync(bugId);
        var person = await _readPersonRepository.GetAsync(request.PersonId);

        existingBug.AssignedTo = person;

        await _writeBugRepository.UpdateAsync(existingBug);
        return Result.NoContent<Unit>();
    }
}
