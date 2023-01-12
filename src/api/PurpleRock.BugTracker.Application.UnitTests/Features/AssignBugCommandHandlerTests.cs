using PurpleRock.BugTracker.Application.Features.AssignBug;

namespace Features;

public class AssignBugCommandHandlerTests
{
    private readonly AssignBugCommandHandler _sut;
    private readonly Mock<IReadBugRepository> _readBugRepository;
    private readonly Mock<IWriteBugRepository> _writeBugRepository;
    private readonly Mock<IReadPersonRepository> _readPersonRepository;
    private readonly Mock<ILogger<AssignBugCommandHandler>> _logger;

    public AssignBugCommandHandlerTests()
    {
        _readBugRepository = new Mock<IReadBugRepository>();
        _writeBugRepository = new Mock<IWriteBugRepository>();
        _readPersonRepository = new Mock<IReadPersonRepository>();
        _logger = new Mock<ILogger<AssignBugCommandHandler>>();

        _sut = new AssignBugCommandHandler(
            _readBugRepository.Object,
            _writeBugRepository.Object,
            _readPersonRepository.Object,
            _logger.Object
        );
    }

    [Fact]
    public async Task Handle_GivenInvalidPersonId_ThrowsNotFoundException()
    {
        // Arrange
        var bugId = "1";
        var personId = "invalid-id";
        var bug = new Bug
        {
            Id = bugId,
        };
        var command = new AssignBugCommand(bugId, personId);

        _readBugRepository.Setup(x => x.GetAsync(bugId)).ReturnsAsync(bug);
        _readPersonRepository.Setup(x => x.GetAsync(personId)).ThrowsAsync(new NotFoundException());

        // Act
        var act = () => _sut.Handle(command, CancellationToken.None);

        // Assert
        await act.ShouldThrowAsync<NotFoundException>();
        _writeBugRepository.Verify(x => x.UpdateAsync(It.IsAny<Bug>()), Times.Never());
    }

    [Fact]
    public async Task Handle_GivenValidCommand_UpdatesBugAndReturnsNoContent()
    {
        // Arrange
        var bugId = "1";
        var personId = "2";
        var bug = new Bug
        {
            Id = bugId,
        };
        var person = new Person
        {
            Id = personId,
        };
        var command = new AssignBugCommand(bugId, personId);

        _readBugRepository.Setup(x => x.GetAsync(bugId)).ReturnsAsync(bug);
        _readPersonRepository.Setup(x => x.GetAsync(personId)).ReturnsAsync(person);

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.Status.ShouldBe(HttpStatusCode.NoContent);
        _writeBugRepository.Verify(x => x.UpdateAsync(It.Is<Bug>(b => b.AssignedTo == person)), Times.Once());
    }
}
