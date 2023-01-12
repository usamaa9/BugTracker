using PurpleRock.BugTracker.Application.Contracts.Requests;
using PurpleRock.BugTracker.Application.Features.ChangePersonName;
using PurpleRock.BugTracker.Application.Features.CreatePerson;

namespace PurpleRock.BugTracker.WebApi.Controllers.v1;

/// <summary>
/// Person Controller.
/// </summary>
[Route("api/v1/person")]
public class PersonV1Controller : BaseV1Controller
{
    public PersonV1Controller(ICommandBus commandBus) : base(commandBus)
    {
    }

    /// <summary>
    /// Adds a person.
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost]
    [Created("Person created")]
    [BadRequest]
    [SwaggerOperation(Tags = new[] { "Person" })]
    public async Task<IActionResult> CreatePersonAsync([Required] CreatePersonRequest request)
    {
        var result = await CommandBus.SendAsync<CreatePersonCommand, Unit>(
            HttpContext, new CreatePersonCommand(request, "/api/v1/person/{personId}"));

        return result.ToActionResult();
    }

    /// <summary>
    /// Changes a person's name.
    /// </summary>
    /// <param name="updatedName">The new name.</param>
    /// <param name="personId">The unique person identifier.</param>
    /// <returns></returns>
    [HttpPost("{personId}/{updatedName}")]
    [NoContent("Person name changed")]
    [BadRequest]
    [SwaggerOperation(Tags = new[] { "Person" })]
    public async Task<IActionResult> ChangePersonNameAsync([Required] string personId, [Required] string updatedName)
    {
        var result = await CommandBus.SendAsync<ChangePersonNameCommand, Unit>(
            HttpContext, new ChangePersonNameCommand(personId, updatedName));

        return result.ToActionResult();
    }
}
