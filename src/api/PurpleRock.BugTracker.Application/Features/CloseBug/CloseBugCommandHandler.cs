namespace PurpleRock.BugTracker.Application.Features.CloseBug;

public class CloseBugCommandHandler : IRequestHandler<CloseBugCommand, Result<Unit>>
{
    private readonly ILogger<CloseBugCommandHandler> _logger;
    private readonly IReadBugRepository _readBugRepository;
    private readonly IWriteBugRepository _writeBugRepository;

    public CloseBugCommandHandler(
        IReadBugRepository readBugRepository,
        IWriteBugRepository writeBugRepository,
        ILogger<CloseBugCommandHandler> logger
    )
    {
        _readBugRepository = readBugRepository;
        _writeBugRepository = writeBugRepository;
        _logger = logger;
    }

    public async Task<Result<Unit>> Handle(CloseBugCommand request, CancellationToken cancellationToken)
    {
        var bugId = request.BugId;

        var existingBug = await _readBugRepository.GetAsync(bugId);

        existingBug.DateClosed = DateTime.UtcNow;

        await _writeBugRepository.UpdateAsync(existingBug);

        return Result.NoContent<Unit>();
    }
}
