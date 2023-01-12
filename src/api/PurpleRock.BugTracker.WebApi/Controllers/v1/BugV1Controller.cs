using PurpleRock.BugTracker.Application.Contracts.Requests;
using PurpleRock.BugTracker.Application.Contracts.Responses;
using PurpleRock.BugTracker.Application.Features.AssignBug;
using PurpleRock.BugTracker.Application.Features.BugDetails;
using PurpleRock.BugTracker.Application.Features.CloseBug;
using PurpleRock.BugTracker.Application.Features.CreateBug;
using PurpleRock.BugTracker.Application.Features.GetAllBugs;

namespace PurpleRock.BugTracker.WebApi.Controllers.v1;

/// <summary>
/// Bug controller.
/// </summary>
[Route("api/v1/bug")]
public class BugV1Controller : BaseV1Controller
{
    /// <summary>
    /// Initializes a new instance of the <see cref="BugV1Controller"/> class.
    /// </summary>
    /// <param name="commandBus"><see cref="ICommandBus"/>.</param>
    public BugV1Controller(ICommandBus commandBus) : base(commandBus)
    {
    }

    /// <summary>
    /// Get bug details.
    /// </summary>
    /// <param name="bugId">A bug's unique identifier.</param>
    /// <returns>Bug details.</returns>
    [HttpGet("{bugId}")]
    [Ok("Bug Details", typeof(BugDetailsResponse))]
    [NotFound(nameof(User))]
    [SwaggerOperation(Tags = new[] { "Bug" })]
    public async Task<IActionResult> GetBugDetailsAsync(string bugId)
    {
        var result = await CommandBus.SendAsync<BugDetailsQuery, BugDetailsResponse>(HttpContext, new BugDetailsQuery(bugId));
        return result.ToActionResult();
    }

    /// <summary>
    /// Get a list of all bugs filtered by status.
    /// </summary>
    /// <returns>List of all bugs.</returns>
    [HttpGet("list")]
    [Ok("List of Bug", typeof(IReadOnlyCollection<BugDetailsResponse>))]
    [SwaggerOperation(Tags = new[] { "Bug" })]
    public async Task<IActionResult> GetAllBugsAsync([FromQuery] string status)
    {
        var query = new GetAllBugsQuery();
        if (status == "open") query.Status = "open";

        var result = await CommandBus.SendAsync<GetAllBugsQuery, IReadOnlyCollection<BugDetailsResponse>>(HttpContext,
            query);
        return result.ToActionResult();
    }

    /// <summary>
    /// Creates a bug.
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost]
    [Created("Bug created")]
    [BadRequest]
    [SwaggerOperation(Tags = new[] { "Bug" })]
    public async Task<IActionResult> CreateBugAsync([Required] CreateBugRequest request)
    {
        var result = await CommandBus.SendAsync<CreateBugCommand, Unit>(
            HttpContext, new CreateBugCommand(request, "/api/v1/bug/{bugId}"));

        return result.ToActionResult();
    }

    /// <summary>
    /// Closes a bug.
    /// </summary>
    /// <param name="bugId">The unique bug identifier.</param>
    /// <returns></returns>
    [HttpPost("{bugId}/close")]
    [NoContent("Bug Closed")]
    [BadRequest]
    [SwaggerOperation(Tags = new[] { "Bug" })]
    public async Task<IActionResult> CloseBugAsync([Required] string bugId)
    {
        var result = await CommandBus.SendAsync<CloseBugCommand, Unit>(
            HttpContext, new CloseBugCommand(bugId));

        return result.ToActionResult();
    }

    /// <summary>
    /// Assigns a bug to a person.
    /// </summary>
    /// <param name="bugId">The unique bug identifier.</param>
    /// <param name="personId">The unique person identifier.</param>
    /// <returns></returns>
    [HttpPost("{bugId}/assign/{personId}")]
    [NoContent("Bug Assigned")]
    [BadRequest]
    [SwaggerOperation(Tags = new[] { "Bug" })]
    public async Task<IActionResult> AssignBugAsync([Required] string bugId, [Required] string personId)
    {
        var result = await CommandBus.SendAsync<AssignBugCommand, Unit>(
            HttpContext, new AssignBugCommand(bugId, personId));

        return result.ToActionResult();
    }
}
