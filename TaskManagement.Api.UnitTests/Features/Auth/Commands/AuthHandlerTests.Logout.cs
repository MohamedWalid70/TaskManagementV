using AutoFixture;
using MediatR;
using Moq;
using Shouldly;
using System.Security.Claims;
using TaskManagement.Application.Features.Auth.Commands.Logout;

namespace TaskManagement.Api.UnitTests.Features.Auth
{
    public partial class AuthHandlersTests
    {
        [Fact]
        public async Task LogoutHandler_WithAccessTokenSentInHeader_ShouldReturnUnit()
        {
            Guid userId = Guid.NewGuid();

            List<Claim> claims = [new Claim(ClaimTypes.NameIdentifier, userId.ToString())];

            SetHttpContextAccessorUserForMock(claims);

            var logoutHandler = _fixture.Build<LogoutCommandHandler>().Create();

            var logoutCommand = _fixture.Build<LogoutCommand>().Create();

            _refreshTokenRepositoryMock.Setup(x => x.RemoveRefreshTokensByUserIdAsync(userId));

            var result = await logoutHandler.Handle(logoutCommand, default);

            result.ShouldBe(Unit.Value);

            _refreshTokenRepositoryMock.Verify(x => x.RemoveRefreshTokensByUserIdAsync(It.Is<Guid>(x => x == userId)), Times.Once);

        }



        [Fact]
        public async Task LogoutHandler_WithNoAccessToken_ShouldReturnWithNoActionDone()
        {
            Guid userId = Guid.NewGuid();

            List<Claim> claims = new();

            SetHttpContextAccessorUserForMock(claims);

            var logoutHandler = _fixture.Build<LogoutCommandHandler>().Create();

            var logoutCommand = _fixture.Build<LogoutCommand>().Create();

            _refreshTokenRepositoryMock.Setup(x => x.RemoveRefreshTokensByUserIdAsync(userId));

            var result = await logoutHandler.Handle(logoutCommand, default);

            result.ShouldBe(Unit.Value);

            _refreshTokenRepositoryMock.Verify(x => x.RemoveRefreshTokensByUserIdAsync(userId), Times.Never);

        }


    }
}
