using PurpleRock.BugTracker.Application.Features.BugDetails;

namespace Features;

public class BugDetailsQueryHandlerTests
{
    private readonly BugDetailsQueryHandler _sut;
    private readonly Mock<IReadBugRepository> _readBugRepository;
    private readonly Mock<IModelMapper<Bug, BugDetailsResponse>> _mapper;

    public BugDetailsQueryHandlerTests()
    {
        _readBugRepository = new Mock<IReadBugRepository>();
        _mapper = new Mock<IModelMapper<Bug, BugDetailsResponse>>();

        _sut = new BugDetailsQueryHandler(_readBugRepository.Object, _mapper.Object);
    }

    [Fact]
    public async Task Handle_ValidRequest_ReturnsExpectedResult()
    {
        // Arrange
        var bugId = "1";
        var testBug = "Test Bug";
        var bug = new Bug { Id = bugId, Title = testBug };
        _readBugRepository.Setup(x => x.GetAsync(It.IsAny<string>())).ReturnsAsync(bug);

        var expectedResponse = new BugDetailsResponse { Id = bugId, Title = testBug };
        _mapper.Setup(x => x.Map(It.IsAny<Bug>())).Returns(expectedResponse);

        var request = new BugDetailsQuery(bugId);

        // Act
        var result = await _sut.Handle(request, default);

        // Assert
        result.Status.ShouldBe(HttpStatusCode.OK);
        result.Value.ShouldBe(expectedResponse);
    }

    [Fact]
    public async Task Handle_BugDoesNotExist_ThrowsNotFoundException()
    {
        // Arrange
        _readBugRepository.Setup(x => x.GetAsync(It.IsAny<string>())).ThrowsAsync(new NotFoundException());

        var request = new BugDetailsQuery("1");

        // Act
        var act = () => _sut.Handle(request, default);

        // Assert
        await act.ShouldThrowAsync<NotFoundException>();
    }
}
