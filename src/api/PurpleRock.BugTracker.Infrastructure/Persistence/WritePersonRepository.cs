namespace PurpleRock.BugTracker.Infrastructure.Persistence;

public class WritePersonRepository : IWritePersonRepository
{
    private readonly IModelMapper<Person, PersonDto> _mapper;
    private readonly IRepository<PersonDto> _repository;

    public WritePersonRepository(
        IRepository<PersonDto> repository,
        IModelMapper<Person, PersonDto> mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<bool> CreateAsync(Person person)
    {
        var dto = _mapper.Map(person);
        _ = await _repository.CreateAsync(dto);

        return true;
    }

    public async Task UpdateAsync(Person person)
    {
        var dto = _mapper.Map(person);
        await _repository.UpdateAsync(dto);
    }
}
