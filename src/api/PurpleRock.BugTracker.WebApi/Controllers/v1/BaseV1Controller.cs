namespace PurpleRock.BugTracker.WebApi.Controllers.v1;

/// <summary>
/// Base controller for all controllers.
/// </summary>
[ApiExplorerSettings(GroupName = "v1")]
[ApiController]
[InternalServerError]
public abstract class BaseV1Controller : ControllerBase
{
    /// <summary>
    /// Initializes a new instance of the <see cref="BaseV1Controller"/> class.
    /// </summary>
    /// <param name="commandBus"><see cref="ICommandBus"/>.</param>
    protected BaseV1Controller(ICommandBus commandBus)
    {
        CommandBus = commandBus;
    }

    /// <summary>
    /// Command bus.
    /// </summary>
    protected ICommandBus CommandBus { get; }
}
