using PurpleRock.BugTracker.Application.Features.CloseBug;

namespace Features;

public class CloseBugCommandHandlerTests
{
    private readonly CloseBugCommandHandler _sut;
    private readonly Mock<IReadBugRepository> _readBugRepository;
    private readonly Mock<IWriteBugRepository> _writeBugRepository;

    public CloseBugCommandHandlerTests()
    {
        _readBugRepository = new Mock<IReadBugRepository>();
        _writeBugRepository = new Mock<IWriteBugRepository>();
        var logger = new Mock<ILogger<CloseBugCommandHandler>>();
        _sut = new CloseBugCommandHandler(_readBugRepository.Object, _writeBugRepository.Object, logger.Object);
    }

    [Fact]
    public async Task Handle_BugExists_ClosesBug()
    {
        // Arrange
        const string bugId = "1";
        var command = new CloseBugCommand(bugId);
        var existingBug = new Bug { Id = bugId, DateClosed = null };
        _readBugRepository.Setup(x => x.GetAsync(command.BugId)).ReturnsAsync(existingBug);

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.Status.ShouldBe(HttpStatusCode.NoContent);

        _writeBugRepository.Verify(x => x.UpdateAsync(existingBug), Times.Once);
    }

    [Fact]
    public async Task Handle_BugDoesNotExist_ReturnsBadRequest()
    {
        // Arrange
        const string bugId = "1";
        var command = new CloseBugCommand(bugId);
        _readBugRepository.Setup(x => x.GetAsync(bugId)).ThrowsAsync(new NotFoundException());

        // Act
        var act = () => _sut.Handle(command, CancellationToken.None);

        // Assert
        await act.ShouldThrowAsync<NotFoundException>();
        _writeBugRepository.Verify(x => x.UpdateAsync(It.IsAny<Bug>()), Times.Never);
    }
}
