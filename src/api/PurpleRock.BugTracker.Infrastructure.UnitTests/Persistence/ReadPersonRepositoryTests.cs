namespace Persistence;

public class ReadPersonRepositoryTests
{
    private readonly Mock<IRepository<PersonDto>> _cosmosRepository;
    private readonly Mock<IModelMapper<PersonDto, Person>> _mapper;
    private readonly ReadPersonRepository _sut;

    public ReadPersonRepositoryTests()
    {
        _cosmosRepository = new Mock<IRepository<PersonDto>>();
        _mapper = new Mock<IModelMapper<PersonDto, Person>>();
        _sut = new ReadPersonRepository(_cosmosRepository.Object, _mapper.Object);
    }

    [Fact]
    public async Task GetAsync_PersonExist_ReturnsPerson()
    {
        // Arrange
        var personId = "1";
        var person = new PersonDto { Id = personId, Name = "John Smith" };
        _cosmosRepository
            .Setup(x => x.GetAsync(personId, null, CancellationToken.None))
            .ReturnsAsync(person);
        _mapper
            .Setup(x => x.Map(It.IsAny<PersonDto>()))
            .Returns((PersonDto x) => new Person { Id = x.Id, Name = x.Name });

        // Act
        var result = await _sut.GetAsync(personId);

        // Assert
        result.ShouldNotBeNull();
        result.Id.ShouldBe(personId);
        result.Name.ShouldBe("John Smith");
    }

    [Fact]
    public async Task GetAsync_PersonDoesNotExist_ThrowsNotFoundException()
    {
        // Arrange
        var exception = new CosmosException(It.IsAny<string>(), HttpStatusCode.NotFound, It.IsAny<int>(), It.IsAny<string>(),
            It.IsAny<double>());
        _cosmosRepository
            .Setup(x => x.GetAsync(It.IsAny<string>(), null, CancellationToken.None))
            .ThrowsAsync(exception);
        // Act
        var act = async () => await _sut.GetAsync("1");

        // Assert
        await act.ShouldThrowAsync<NotFoundException>();
    }
}
