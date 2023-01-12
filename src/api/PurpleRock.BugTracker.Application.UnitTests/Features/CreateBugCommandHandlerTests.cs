using PurpleRock.BugTracker.Application.Contracts.Requests;
using PurpleRock.BugTracker.Application.Features.CreateBug;

namespace Features;

public class CreateBugCommandHandlerTests
{
    private readonly Mock<IWriteBugRepository> _writeBugRepository;
    private readonly CreateBugCommandHandler _sut;

    public CreateBugCommandHandlerTests()
    {
        _writeBugRepository = new Mock<IWriteBugRepository>();
        var logger = new Mock<ILogger<CreateBugCommandHandler>>().Object;
        _sut = new CreateBugCommandHandler(_writeBugRepository.Object, logger);
    }

    [Fact]
    public async Task Handle_CreateBugCommand_Returns_CreatedResult()
    {
        // Arrange
        var request = new CreateBugCommand(new CreateBugRequest
        {
            Title = It.IsAny<string>(),
            Description = It.IsAny<string>()
        });
        _writeBugRepository.Setup(x => x.CreateAsync(It.IsAny<Bug>())).ReturnsAsync(true);

        // Act
        var result = await _sut.Handle(request, CancellationToken.None);

        // Assert
        result.Status.ShouldBe(HttpStatusCode.Created);
    }

    [Fact]
    public async Task Handle_CreateBugCommand_Returns_BadRequestResult()
    {
        // Arrange
        var request = new CreateBugCommand(new CreateBugRequest
        {
            Title = It.IsAny<string>(),
            Description = It.IsAny<string>()
        });
        _writeBugRepository.Setup(x => x.CreateAsync(It.IsAny<Bug>())).ReturnsAsync(false);

        // Act
        var result = await _sut.Handle(request, CancellationToken.None);

        // Assert
        result.Status.ShouldBe(HttpStatusCode.BadRequest);
    }
}
