namespace PurpleRock.BugTracker.Infrastructure.Persistence;

/// <inheritdoc />
public class ReadPersonRepository : IReadPersonRepository
{
    private readonly IModelMapper<PersonDto, Person> _mapper;
    private readonly IRepository<PersonDto> _repository;

    public ReadPersonRepository(
        IRepository<PersonDto> repository,
        IModelMapper<PersonDto, Person> mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<Person?> GetAsync(string personId)
    {
        try
        {
            var dto = await _repository.GetAsync(personId);

            return _mapper.Map(dto);
        }
        catch (CosmosException e) when (e.StatusCode == HttpStatusCode.NotFound)
        {
            throw new NotFoundException(nameof(Person), personId);
        }
    }
}
