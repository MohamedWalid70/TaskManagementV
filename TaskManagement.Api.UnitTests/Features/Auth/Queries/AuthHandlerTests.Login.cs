using AutoFixture;
using Moq;
using Shouldly;
using TaskManagement.Application.Features.Auth.Queries.Login;
using TaskManagement.Domain.Entities;

namespace TaskManagement.Api.UnitTests.Features.Auth
{
    public partial class AuthHandlersTests
    {
        [Fact]
        public async Task LoginHandler_WithValidCommand_ShouldReturnTheCreatedTokens()
        {
            var loginHandler = _fixture.Build<LoginQueryHandler>().Create();

            var loginQuery = _fixture.Build<LoginQuery>().Without(x => x.SharedUser).Create();

            loginQuery.SharedUser = _fixture.Create<UserEntity>();

            var userRoles = new List<string>() { loginQuery.SharedUser.Role };

            var jwtToken = _fixture.Build<string>().Create();

            var refreshToken = _fixture.Create<RefreshTokenEntity>();

            typeof(RefreshTokenEntity).GetProperty(nameof(RefreshTokenEntity.UserId))?.SetValue(refreshToken, loginQuery.SharedUser.Id);

            _tokenGeneratorMock.Setup(x => x.GenerateJwtToken(loginQuery.SharedUser, userRoles)).Returns(jwtToken);

            _tokenGeneratorMock.Setup(x => x.GenerateRefreshToken(loginQuery.SharedUser.Id)).Returns(refreshToken);

            _dbContextMock.Setup(x => x.SaveChangesAsync());

            _refreshTokenRepositoryMock.Setup(x => x.AddRefreshTokenAsync(refreshToken));


            var result = await loginHandler.Handle(loginQuery, default);

            result.AccessToken.ShouldBe(jwtToken);

            result.RefreshToken.ShouldBe(refreshToken.Token);

            _tokenGeneratorMock.Verify(x => x.GenerateJwtToken(It.Is<UserEntity>(x => x.Id == loginQuery.SharedUser.Id), userRoles), Times.Once);

            _tokenGeneratorMock.Verify(x => x.GenerateRefreshToken(It.Is<Guid>(x => x == loginQuery.SharedUser.Id)), Times.Once);

            _refreshTokenRepositoryMock.Verify(x => x.AddRefreshTokenAsync(refreshToken), Times.Once);

            _dbContextMock.Verify(x => x.SaveChangesAsync(), Times.Once);

        }

    }
}
