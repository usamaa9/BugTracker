namespace Persistence;

public class ReadBugRepositoryTests
{
    private readonly Mock<IRepository<BugDto>> _cosmosRepository;
    private readonly Mock<IReadPersonRepository> _readPersonRepository;
    private readonly Mock<IModelMapper<BugDto, Bug>> _mapper;
    private readonly ReadBugRepository _sut;

    public ReadBugRepositoryTests()
    {
        _cosmosRepository = new Mock<IRepository<BugDto>>();
        _readPersonRepository = new Mock<IReadPersonRepository>();
        _mapper = new Mock<IModelMapper<BugDto, Bug>>();
        _sut = new ReadBugRepository(_cosmosRepository.Object, _readPersonRepository.Object, _mapper.Object);
    }

    [Fact]
    public async Task GetAllAsync_BugsExist_ReturnsAllBugs()
    {
        // Arrange
        var bug1 = new BugDto { Id = "1", Title = "bug1", PersonAssignedId = "1" };
        var bug2 = new BugDto { Id = "2", Title = "bug2" };
        _cosmosRepository
            .Setup(x => x.GetByQueryAsync("SELECT * from c", CancellationToken.None))
            .ReturnsAsync(new List<BugDto> { bug1, bug2 });
        _mapper
            .Setup(x => x.Map(It.IsAny<BugDto>()))
            .Returns((BugDto x) => new Bug { Id = x.Id, Title = x.Title, AssignedTo = new Person() { Id = x.PersonAssignedId } });

        // Act
        var result = await _sut.GetAllAsync();

        // Assert
        result.Count.ShouldBe(2);
    }

    [Fact]
    public async Task GetAllAsync_NoBugs_ReturnsEmptyList()
    {
        // Arrange
        _cosmosRepository
            .Setup(x => x.GetByQueryAsync("SELECT * from c", CancellationToken.None))
            .ReturnsAsync(new List<BugDto>());

        // Act
        var result = await _sut.GetAllAsync();

        // Assert
        result.ShouldBeEmpty();
    }

    [Fact]
    public async Task GetAsync_BugExist_ReturnsBug()
    {
        // Arrange
        var bugId = "1";
        var bug = new BugDto { Id = bugId, Title = "bug1", PersonAssignedId = "1" };
        _cosmosRepository
            .Setup(x => x.GetAsync(bugId, null, CancellationToken.None))
            .ReturnsAsync(bug);
        _mapper
            .Setup(x => x.Map(It.IsAny<BugDto>()))
            .Returns((BugDto x) => new Bug { Id = x.Id, Title = x.Title });
        _readPersonRepository
            .Setup(x => x.GetAsync("1"))
            .ReturnsAsync(new Person { Id = "1" });

        // Act
        var result = await _sut.GetAsync(bugId);

        // Assert
        result.ShouldNotBeNull();
        result.Id.ShouldBe(bugId);
        result.Title.ShouldBe("bug1");
        result.AssignedTo!.Id.ShouldBe("1");
    }

    [Fact]
    public async Task GetAsync_BugDoesNotExist_ThrowsNotFoundException()
    {
        // Arrange
        var exception = new CosmosException(It.IsAny<string>(), HttpStatusCode.NotFound, It.IsAny<int>(), It.IsAny<string>(), It.IsAny<double>());
        _cosmosRepository
            .Setup(x => x.GetAsync(It.IsAny<string>(), null, CancellationToken.None))
            .ThrowsAsync(exception);

        // Act

        var act = () => _sut.GetAsync(It.IsAny<string>());

        // Assert
        await act.ShouldThrowAsync<NotFoundException>();
    }
}
