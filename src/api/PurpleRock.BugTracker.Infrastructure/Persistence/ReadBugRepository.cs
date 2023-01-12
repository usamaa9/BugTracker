namespace PurpleRock.BugTracker.Infrastructure.Persistence;

public class ReadBugRepository : IReadBugRepository
{
    private readonly IModelMapper<BugDto, Bug> _mapper;
    private readonly IReadPersonRepository _readPersonRepository;
    private readonly IRepository<BugDto> _repository;

    public ReadBugRepository(
        IRepository<BugDto> repository,
        IReadPersonRepository readPersonRepository,
        IModelMapper<BugDto, Bug> mapper)
    {
        _repository = repository;
        _mapper = mapper;
        _readPersonRepository = readPersonRepository;
    }

    public async Task<IReadOnlyCollection<Bug>> GetAllAsync()
    {
        var dtos = await _repository.GetByQueryAsync("SELECT * from c");

        var bos = new List<Bug>();

        foreach (var dto in dtos)
        {
            var bo = _mapper.Map(dto);
            if (dto.PersonAssignedId != null) bo.AssignedTo = await _readPersonRepository.GetAsync(dto.PersonAssignedId);
            bos.Add(bo);
        }

        return bos;
    }

    public async Task<Bug> GetAsync(string bugId)
    {
        try
        {
            var dto = await _repository.GetAsync(bugId);
            var bo = _mapper.Map(dto);
            if (dto.PersonAssignedId != null) bo.AssignedTo = await _readPersonRepository.GetAsync(dto.PersonAssignedId);

            return bo;
        }
        catch (CosmosException e) when (e.StatusCode == HttpStatusCode.NotFound)
        {
            throw new NotFoundException(nameof(Bug), bugId);
        }
    }
}
