namespace PurpleRock.BugTracker.Application.Features.BugDetails;

public class BugDetailsQueryHandler : IRequestHandler<BugDetailsQuery, Result<BugDetailsResponse>>
{
    private readonly IModelMapper<Bug, BugDetailsResponse> _mapper;
    private readonly IReadBugRepository _readBugRepository;

    public BugDetailsQueryHandler(
        IReadBugRepository readBugRepository,
        IModelMapper<Bug, BugDetailsResponse> mapper)
    {
        _readBugRepository = readBugRepository;
        _mapper = mapper;
    }

    public async Task<Result<BugDetailsResponse>> Handle(BugDetailsQuery request, CancellationToken cancellationToken)
    {
        var bugId = request.BugId;
        var bugDetails = await _readBugRepository.GetAsync(bugId);
        var response = _mapper.Map(bugDetails);
        return Result.Ok(response);
    }
}
