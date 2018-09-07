using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Src.Features.User;

namespace Src.Controllers
{
    [Route("api/users")]
    public class UserController : Controller
    {
        private readonly IMediator _mediator;

        public UserController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet, Route("{userId}")]
        public async Task<IActionResult> GetUser([FromRoute] Guid userId)
        {

            var getUser = new GetUser.Query
            {
                Id = userId
            };

            var result = await _mediator.Send(getUser);

            return Ok(result);
        }
    }
}