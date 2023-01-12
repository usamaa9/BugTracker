using PurpleRock.BugTracker.Application.Features.GetAllBugs;

namespace Features;

public class GetAllBugsQueryHandlerTests
{
    private readonly Mock<IModelMapper<Bug, BugDetailsResponse>> _mapper;
    private readonly Mock<IReadBugRepository> _readBugRepository;
    private readonly GetAllBugsQueryHandler _sut;

    public GetAllBugsQueryHandlerTests()
    {
        _mapper = new Mock<IModelMapper<Bug, BugDetailsResponse>>();
        _readBugRepository = new Mock<IReadBugRepository>();

        _sut = new GetAllBugsQueryHandler(_readBugRepository.Object, _mapper.Object);
    }

    [Fact]
    public async Task Handle_StatusAll_ReturnsFullListOfBugs()
    {
        // Arrange
        var bugs = new List<Bug>()
        {
            new() { Id = "1" },
            new() { Id = "2", DateClosed = DateTime.Now}
        };
        var query = new GetAllBugsQuery() { Status = "All" };
        _readBugRepository.Setup(x => x.GetAllAsync()).ReturnsAsync(bugs);

        // Act
        var result = await _sut.Handle(query, default);

        // Assert
        result.Value.ShouldNotBeNull();
        result.Value.Count.ShouldBe(2);
    }

    [Fact]
    public async Task Handle_StatusOpen_ReturnsListOfOpenBugs()
    {
        // Arrange
        var bugs = new List<Bug>()
        {
            new() { Id = "1" },
            new() { Id = "2", DateClosed = DateTime.Now}
        };
        var query = new GetAllBugsQuery() { Status = "Open" };
        _readBugRepository.Setup(x => x.GetAllAsync()).ReturnsAsync(bugs);

        // Act
        var result = await _sut.Handle(query, default);

        // Assert
        result.Value.ShouldNotBeNull();
        result.Value.Count.ShouldBe(1);
    }
}
