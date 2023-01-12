namespace Persistence;

public class WritePersonRepositoryTests
{
    private readonly Mock<IRepository<PersonDto>> _repository;
    private readonly Mock<IModelMapper<Person, PersonDto>> _mapper;
    private readonly WritePersonRepository _sut;

    public WritePersonRepositoryTests()
    {
        _repository = new Mock<IRepository<PersonDto>>();
        _mapper = new Mock<IModelMapper<Person, PersonDto>>();
        _sut = new WritePersonRepository(_repository.Object, _mapper.Object);
    }

    [Fact]
    public async Task CreateAsync_Person_ReturnsTrue()
    {
        // Arrange
        var person = new Person { Id = "1", Name = "Person1" };
        var dto = new PersonDto { Id = "1", Name = "Person1" };
        _mapper.Setup(x => x.Map(person)).Returns(dto);

        // Act
        var result = await _sut.CreateAsync(person);

        // Assert
        result.ShouldBeTrue();
        _repository.Verify(x => x.CreateAsync(dto, CancellationToken.None), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_Person_UpdatesPerson()
    {
        // Arrange
        var person = new Person { Id = "1", Name = "John Doe" };
        var personDto = new PersonDto { Id = "1", Name = "Jane Doe" };
        _mapper.Setup(x => x.Map(It.IsAny<Person>())).Returns(personDto);
        _repository.Setup(x => x.UpdateAsync(personDto, false, CancellationToken.None))
            .ReturnsAsync(personDto)
            .Verifiable();

        // Act
        await _sut.UpdateAsync(person);

        // Assert
        _mapper.Verify(x => x.Map(It.Is<Person>(p => p.Id == "1" && p.Name == "John Doe")), Times.Once());
        _repository.Verify(x => x.UpdateAsync(personDto, false, CancellationToken.None), Times.Once());
    }
}
