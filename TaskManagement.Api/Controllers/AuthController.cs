using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using TaskManagement.Api.Models.Auth;
using TaskManagement.Application.Features.Auth.Commands.Logout;
using TaskManagement.Application.Features.Auth.Queries.Common;
using TaskManagement.Application.Features.Auth.Queries.Login;
using TaskManagement.Application.Features.Auth.Queries.RefreshToken;
using TaskManagement.Common.GenericResponses;

namespace TaskManagement.Api.Controllers
{
    [Route("api/v1/auth")]
    [ApiController]
    public class AuthController(IMapper mapper, ISender sender) : ControllerBase
    {
        private readonly IMapper _mapper = mapper;
        private readonly ISender _sender = sender;

        [HttpPost]
        [ProducesResponseType(typeof(GenericResult<AuthQueryResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(GenericResult), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<AuthQueryResponse>> Login([FromBody] LoginQueryParam loginQueryParam)
        {
            var loginQuery = _mapper.Map<LoginQuery>(loginQueryParam);

            var loginQueryResponse = await _sender.Send(loginQuery);

            var genericResult = new GenericResult<AuthQueryResponse> { StatusCode = StatusCodes.Status200OK, Data = loginQueryResponse };

            return Ok(genericResult);
        }

        [HttpPost("refresh")]
        [ProducesResponseType(typeof(GenericResult<AuthQueryResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(GenericResult), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<AuthQueryResponse>> Refresh([FromBody] string refreshToken)
        {

            var loginQueryResponse = await _sender.Send(new RefreshTokenQuery { RefreshToken = refreshToken } );

            var genericResult = new GenericResult<AuthQueryResponse> { StatusCode = StatusCodes.Status200OK, Data = loginQueryResponse };

            return Ok(genericResult);
        }

        [HttpPost("logout")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(GenericResult), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> Logout()
        {
            await _sender.Send(new LogoutCommand());

            return Ok();
        }
    }
}
