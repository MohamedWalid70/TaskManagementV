using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManagement.Api.Models.Auth;
using TaskManagement.Api.Models.Users;
using TaskManagement.Application.Features.Auth.Queries.Common;
using TaskManagement.Application.Features.Auth.Queries.Login;
using TaskManagement.Application.Features.Common;
using TaskManagement.Application.Features.Users.Commands.CreateUser;
using TaskManagement.Application.Features.Users.Commands.DeleteUser;
using TaskManagement.Application.Features.Users.Queries.GetPaginatedUsers;
using TaskManagement.Application.Features.Users.Queries.GetUserById;
using TaskManagement.Application.Features.Users.Queries.GetUserProfile;
using TaskManagement.Common.GenericResponses;

namespace TaskManagement.Api.Controllers
{
    [Route("api/v1/users")]
    [ApiController]
    public class UserController(IMapper mapper, ISender sender) : ControllerBase
    {
        private readonly IMapper _mapper = mapper;
        private readonly ISender _sender = sender;

        [HttpPost]
        [ProducesResponseType(typeof(IdResponse<int>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(GenericResult), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IdResponse<int>>> CreateUser([FromBody] CreateUserCommandParam createUserCommandParam)
        {
            var createUserCommand = _mapper.Map<CreateUserCommand>(createUserCommandParam);

            var newlyCreatedId = await _sender.Send(createUserCommand);

            return CreatedAtAction(nameof(GetUserById), new { id = newlyCreatedId.Id }, newlyCreatedId);

        }

        [Authorize(Roles = "Admin")]
        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(GenericResult<GetUserByIdQueryResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(GenericResult), StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<GenericResult<GetUserByIdQueryResponse>>> GetUserById(Guid id)
        {
            var getUserByIdQueryResponse = await _sender.Send(new GetUserByIdQuery(id));

            var result = new GenericResult<GetUserByIdQueryResponse> { StatusCode = StatusCodes.Status200OK, Data = getUserByIdQueryResponse };

            return Ok(result);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IAsyncEnumerable<GetUserByIdQueryResponse> GetPaginatedUsers(int pageNumber = 1, int pageSize = 5)
        {
            return _sender.CreateStream(new GetPaginatedUsersQuery(pageNumber, pageSize));
        }

        [Authorize(Roles = "Admin,User")]
        [HttpGet("me")]
        [ProducesResponseType(typeof(GenericResult<AuthQueryResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(GenericResult), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<GenericResult<GetCurrentUserProfileQueryResponse>>> GetUserProfile()
        {
            var getCurrentUserProfileQuery = new GetCurrentUserProfileQuery();

            var response = await _sender.Send(getCurrentUserProfileQuery);

            var genericResult = new GenericResult<GetCurrentUserProfileQueryResponse> { StatusCode = StatusCodes.Status200OK, Data = response };

            return Ok(genericResult);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(GenericResult), StatusCodes.Status404NotFound)]
        public async Task<ActionResult> DeleteUser(Guid id)
        {
            await _sender.Send(new DeleteUserCommand { EntityId = id });
            return NoContent();

        }
    }
}
