namespace PurpleRock.BugTracker.Application.Features.GetAllBugs;

public class GetAllBugsQueryHandler : IRequestHandler<GetAllBugsQuery, Result<IReadOnlyCollection<BugDetailsResponse>>>
{
    private readonly IModelMapper<Bug, BugDetailsResponse> _mapper;
    private readonly IReadBugRepository _readBugRepository;

    public GetAllBugsQueryHandler(
        IReadBugRepository readBugRepository,
        IModelMapper<Bug, BugDetailsResponse> mapper)
    {
        _readBugRepository = readBugRepository;
        _mapper = mapper;
    }

    public async Task<Result<IReadOnlyCollection<BugDetailsResponse>>> Handle(GetAllBugsQuery request,
        CancellationToken cancellationToken)
    {
        var bugs = await _readBugRepository.GetAllAsync();
        if (request.Status!.ToLower() == "open") bugs = bugs.Where(x => x.IsOpen).ToArray();
        var response = bugs.Select(x => _mapper.Map(x)).ToArray();
        return Result.Ok((IReadOnlyCollection<BugDetailsResponse>)response);
    }
}
