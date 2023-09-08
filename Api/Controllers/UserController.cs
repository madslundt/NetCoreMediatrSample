using Components.UserComponents.Commands;
using Components.UserComponents.Queries;
using DataModel.Models.Users;
using Infrastructure.CQRS.Commands;
using Infrastructure.CQRS.Queries;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("/api/users")]
public class UserController : ControllerBase
{
    private readonly ICommandBus _commandBus;
    private readonly IQueryBus _queryBus;

    public UserController(IQueryBus queryBus, ICommandBus commandBus)
    {
        _queryBus = queryBus;
        _commandBus = commandBus;
    }

    [HttpGet]
    [Route("{userId}", Name = nameof(GetUser))]
    public async Task<ActionResult<GetUser.Result>> GetUser([FromRoute] UserId userId,
        CancellationToken cancellationToken)
    {
        var query = new GetUser.Query
        {
            UserId = userId
        };
        var result = await _queryBus.Send(query, cancellationToken);

        return Ok(result);
    }

    [HttpPost]
    [Route("", Name = nameof(CreateUser))]
    public async Task<ActionResult<CreateUser.Result>> CreateUser(
        [FromBody] CreateUser.Command command,
        CancellationToken cancellationToken
    )
    {
        var result = await _commandBus.Send(command, cancellationToken);

        return Created($"/api/users/{result.Id}", result);
    }

    [HttpDelete]
    [Route("{userId}", Name = nameof(DeleteUser))]
    public async Task<IActionResult> DeleteUser(
        [FromRoute] string userId,
        CancellationToken cancellationToken
    )
    {
        var command = new DeleteUser.Command
        {
            UserId = userId
        };

        await _commandBus.Send(command, cancellationToken);

        return NoContent();
    }
}