using PurpleRock.BugTracker.Application.Features.ChangePersonName;

namespace Features;

public class ChangePersonNameCommandHandlerTests
{
    private readonly ChangePersonNameCommandHandler _sut;
    private readonly Mock<IReadPersonRepository> _readPersonRepository;
    private readonly Mock<IWritePersonRepository> _writePersonRepository;
    private readonly Mock<ILogger<ChangePersonNameCommandHandler>> _logger;

    public ChangePersonNameCommandHandlerTests()
    {
        _readPersonRepository = new Mock<IReadPersonRepository>();
        _writePersonRepository = new Mock<IWritePersonRepository>();
        _logger = new Mock<ILogger<ChangePersonNameCommandHandler>>();
        _sut = new ChangePersonNameCommandHandler(
            _readPersonRepository.Object,
            _writePersonRepository.Object,
            _logger.Object
        );
    }

    [Fact]
    public async Task Handle_ExistingPerson_NameUpdated()
    {
        //Arrange
        const string personId = "1";
        const string newName = "New Name";
        var command = new ChangePersonNameCommand(personId, newName);
        var existingPerson = new Person { Id = personId, Name = "Old Name" };
        _readPersonRepository.Setup(x => x.GetAsync(command.PersonId!)).ReturnsAsync(existingPerson);

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        _writePersonRepository.Verify(x => x.UpdateAsync(existingPerson), Times.Once);
        result.Status.ShouldBe(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task Handle_PersonDoesNotExist_ThrowsNotFoundException()
    {
        //Arrange
        var command = new ChangePersonNameCommand(It.IsAny<string>(), It.IsAny<string>());

        _readPersonRepository.Setup(x => x.GetAsync(It.IsAny<string>())).ThrowsAsync(new NotFoundException());

        // Act
        var act = () => _sut.Handle(command, CancellationToken.None);

        // Assert
        await act.ShouldThrowAsync<NotFoundException>();

        _writePersonRepository.Verify(x => x.UpdateAsync(It.IsAny<Person>()), Times.Never);
    }
}
