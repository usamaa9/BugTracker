using Mapster;

namespace PurpleRock.BugTracker.Infrastructure.Persistence.Mappers;

public class BugDtoMapper : IModelMapper<Bug, BugDto>
{
    public BugDto Map(Bug source)
    {
        var dto = source.Adapt<BugDto>();
        dto.PersonAssignedId = source.AssignedTo?.Id;
        return dto;
    }
}
