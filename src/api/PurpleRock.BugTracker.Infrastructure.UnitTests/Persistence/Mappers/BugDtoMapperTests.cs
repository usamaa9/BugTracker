using PurpleRock.BugTracker.Infrastructure.Persistence.Mappers;

namespace Persistence.Mappers;

public class BugDtoMapperTests
{
    private readonly BugDtoMapper _sut;

    public BugDtoMapperTests()
    {
        _sut = new BugDtoMapper();
    }

    [Fact]
    public void Map_Bug_MapsToExpectedDto()
    {
        // Arrange
        var bug = new Bug
        {
            Id = "1",
            Title = "Test Bug",
            Description = "This is a test bug",
            AssignedTo = new Person
            {
                Id = "2"
            },
            DateOpened = new DateTime(2000, 1, 2),
            DateClosed = new DateTime(2000, 2, 2),
        };

        // Act
        var result = _sut.Map(bug);

        // Assert
        result.Id.ShouldBe(bug.Id);
        result.Title.ShouldBe(bug.Title);
        result.Description.ShouldBe(bug.Description);
        result.DateOpened.ShouldBe(bug.DateOpened);
        result.DateClosed.ShouldBe(bug.DateClosed);
        result.PersonAssignedId.ShouldBe(bug.AssignedTo?.Id);
    }
}
