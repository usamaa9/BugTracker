namespace PurpleRock.BugTracker.Infrastructure.Persistence;

public class WriteBugRepository : IWriteBugRepository
{
    private readonly IModelMapper<Bug, BugDto> _mapper;
    private readonly IRepository<BugDto> _repository;

    public WriteBugRepository(
        IRepository<BugDto> repository,
        IModelMapper<Bug, BugDto> mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<bool> CreateAsync(Bug bug)
    {
        var dto = _mapper.Map(bug);
        _ = await _repository.CreateAsync(dto);

        return true;
    }

    public async Task UpdateAsync(Bug bug)
    {
        var dto = _mapper.Map(bug);
        await _repository.UpdateAsync(dto);
    }
}
