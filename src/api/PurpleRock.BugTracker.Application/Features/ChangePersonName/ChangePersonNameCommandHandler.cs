namespace PurpleRock.BugTracker.Application.Features.ChangePersonName;

public class ChangePersonNameCommandHandler : IRequestHandler<ChangePersonNameCommand, Result<Unit>>
{
    private readonly ILogger<ChangePersonNameCommandHandler> _logger;
    private readonly IReadPersonRepository _readPersonRepository;
    private readonly IWritePersonRepository _writePersonRepository;

    public ChangePersonNameCommandHandler(
        IReadPersonRepository readPersonRepository,
        IWritePersonRepository writePersonRepository,
        ILogger<ChangePersonNameCommandHandler> logger
    )
    {
        _readPersonRepository = readPersonRepository;
        _writePersonRepository = writePersonRepository;
        _logger = logger;
    }

    public async Task<Result<Unit>> Handle(ChangePersonNameCommand request, CancellationToken cancellationToken)
    {
        var existingPerson = await _readPersonRepository.GetAsync(request.PersonId!);
        existingPerson!.Name = request.Name;

        await _writePersonRepository.UpdateAsync(existingPerson);

        return Result.NoContent<Unit>();
    }
}
