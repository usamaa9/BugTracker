using PurpleRock.BugTracker.Application.Contracts.Requests;
using PurpleRock.BugTracker.Application.Features.CreatePerson;

namespace Features;

public class CreatePersonCommandHandlerTests
{
    private readonly Mock<ILogger<CreatePersonCommandHandler>> _logger;
    private readonly Mock<IWritePersonRepository> _personRepository;
    private readonly CreatePersonCommandHandler _sut;

    public CreatePersonCommandHandlerTests()
    {
        _logger = new Mock<ILogger<CreatePersonCommandHandler>>();
        _personRepository = new Mock<IWritePersonRepository>();
        _sut = new CreatePersonCommandHandler(_personRepository.Object, _logger.Object);
    }

    [Fact]
    public async Task Handle_ValidRequest_ReturnsCreated()
    {
        // Arrange
        var command = new CreatePersonCommand(new CreatePersonRequest()
        {
            Name = It.IsAny<string>()
        });

        _personRepository.Setup(x => x.CreateAsync(It.IsAny<Person>())).ReturnsAsync(true);

        // Act
        var result = await _sut.Handle(command, default);

        // Assert
        result.Status.ShouldBe(HttpStatusCode.Created);
    }

    [Fact]
    public async Task Handle_CreatePersonCommand_Returns_BadRequestResult()
    {
        // Arrange
        var command = new CreatePersonCommand(new CreatePersonRequest()
        {
            Name = It.IsAny<string>()
        });

        _personRepository.Setup(x => x.CreateAsync(It.IsAny<Person>())).ReturnsAsync(false);

        // Act
        var result = await _sut.Handle(command, default);

        // Assert
        result.Status.ShouldBe(HttpStatusCode.BadRequest);
    }
}
