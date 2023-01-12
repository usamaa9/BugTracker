namespace Persistence;

public class WriteBugRepositoryTests
{
    private readonly Mock<IModelMapper<Bug, BugDto>> _mapper;
    private readonly Mock<IRepository<BugDto>> _repository;
    private readonly WriteBugRepository _sut;

    public WriteBugRepositoryTests()
    {
        _mapper = new Mock<IModelMapper<Bug, BugDto>>();
        _repository = new Mock<IRepository<BugDto>>();
        _sut = new WriteBugRepository(_repository.Object, _mapper.Object);
    }

    [Fact]
    public async Task CreateAsync_ValidBug_ReturnsTrue()
    {
        // Arrange
        var bug = new Bug { Id = "1", Title = "Bug1", Description = "Description of bug1" };
        var bugDto = new BugDto { Id = "1", Title = "Bug1", Description = "Description of bug1" };
        _mapper.Setup(x => x.Map(bug)).Returns(bugDto);

        // Act
        var result = await _sut.CreateAsync(bug);

        // Assert
        result.ShouldBeTrue();
    }

    [Fact]
    public async Task UpdateAsync_ValidBug_UpdateCalled()
    {
        // Arrange
        var bug = new Bug { Id = "1", Title = "Bug1", Description = "Description of bug1" };
        var bugDto = new BugDto { Id = "1", Title = "Bug1", Description = "Description of bug1" };
        _mapper.Setup(x => x.Map(It.IsAny<Bug>())).Returns(bugDto);

        // Act
        await _sut.UpdateAsync(bug);

        // Assert
        _repository.Verify(x => x.UpdateAsync(bugDto, false, CancellationToken.None), Times.Once);
    }
}
