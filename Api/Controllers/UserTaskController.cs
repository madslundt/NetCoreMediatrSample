using Api.Dtos.UserTaskDtos;
using Components.UserTaskComponents.Commands;
using Components.UserTaskComponents.Queries;
using DataModel.Models.UserTasks;
using Infrastructure.CQRS.Commands;
using Infrastructure.CQRS.Queries;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("/api/user-tasks")]
public class UserTaskController : ControllerBase
{
    private readonly ICommandBus _commandBus;
    private readonly IQueryBus _queryBus;

    public UserTaskController(IQueryBus queryBus, ICommandBus commandBus)
    {
        _queryBus = queryBus;
        _commandBus = commandBus;
    }

    [HttpPost]
    [Route("", Name = nameof(CreateUserTask))]
    public async Task<ActionResult<CreateUserTask.Result>> CreateUserTask(
        [FromBody] CreateUserTask.Command command,
        CancellationToken cancellationToken
    )
    {
        var result = await _commandBus.Send(command, cancellationToken);

        return Created($"/api/user-tasks/{result.Id}", result);
    }

    [HttpGet]
    [Route("{userTaskId}", Name = nameof(GetUserTask))]
    public async Task<ActionResult<GetUserTask.Result>> GetUserTask(
        [FromRoute] UserTaskId userTaskId,
        CancellationToken cancellationToken
    )
    {
        var query = new GetUserTask.Query
        {
            UserTaskId = userTaskId
        };
        var result = await _queryBus.Send(query, cancellationToken);

        return Ok(result);
    }

    [HttpDelete]
    [Route("{userTaskId}", Name = nameof(DeleteUserTask))]
    public async Task<IActionResult> DeleteUserTask(
        [FromRoute] string userTaskId,
        CancellationToken cancellationToken
    )
    {
        var command = new DeleteUserTask.Command
        {
            UserTaskId = userTaskId
        };
        await _commandBus.Send(command, cancellationToken);

        return NoContent();
    }

    [HttpPatch]
    [Route("{userTaskId}", Name = nameof(UpdateUserTask))]
    public async Task<IActionResult> UpdateUserTask(
        [FromRoute] UserTaskId userTaskId,
        [FromBody] UpdateUserTaskBodyDto body,
        CancellationToken cancellationToken
    )
    {
        var command = new UpdateUserTask.Command
        {
            UserTaskId = userTaskId,
            AssignToUserId = body.AssignToUserId
        };
        await _commandBus.Send(command, cancellationToken);

        return NoContent();
    }
}