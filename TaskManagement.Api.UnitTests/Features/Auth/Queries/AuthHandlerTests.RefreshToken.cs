using AutoFixture;
using Moq;
using Shouldly;
using TaskManagement.Application.Features.Auth.Queries.RefreshToken;
using TaskManagement.Domain.Entities;

namespace TaskManagement.Api.UnitTests.Features.Auth
{
    public partial class AuthHandlersTests
    {
        [Fact]
        public async Task RefreshTokenHandler_WithValidCommand_ShouldReturnTheCreatedTokens()
        {
            var refreshHandler = _fixture.Build<RefreshTokenQueryHandler>().Create();

            var refreshQuery = _fixture.Build<RefreshTokenQuery>().Create();

            var jwtToken = _fixture.Build<string>().Create();
            var refreshToken = _fixture.Create<RefreshTokenEntity>();

            typeof(RefreshTokenEntity).GetProperty(nameof(RefreshTokenEntity.UserId))?.SetValue(refreshToken, refreshQuery.SharedTokenEntity.UserId);

            var user = _fixture.Create<UserEntity>();

            typeof(UserEntity).GetProperty(nameof(UserEntity.Id))?.SetValue(user, refreshQuery.SharedTokenEntity.UserId);

            var userRoles = new List<string>() { user.Role };

            _userRepositoryMock.Setup(x => x.GetUserByIdAsync(refreshQuery.SharedTokenEntity.UserId)).ReturnsAsync(user);

            _tokenGeneratorMock.Setup(x => x.GenerateJwtToken(user, userRoles)).Returns(jwtToken);

            _tokenGeneratorMock.Setup(x => x.GenerateRefreshToken(refreshQuery.SharedTokenEntity.UserId)).Returns(refreshToken);

            _dbContextMock.Setup(x => x.SaveChangesAsync());


            var result = await refreshHandler.Handle(refreshQuery, default);

            result.AccessToken.ShouldBe(jwtToken);

            result.RefreshToken.ShouldBe(refreshToken.Token);

            _userRepositoryMock.Verify(x => x.GetUserByIdAsync(It.Is<Guid>(id => id == refreshQuery.SharedTokenEntity.UserId)), Times.Once);

            _tokenGeneratorMock.Verify(x => x.GenerateJwtToken(It.Is<UserEntity>(x => x.Id == refreshQuery.SharedTokenEntity.UserId), userRoles), Times.Once);

            _tokenGeneratorMock.Verify(x => x.GenerateRefreshToken(It.Is<Guid>(x => x == refreshQuery.SharedTokenEntity.UserId)), Times.Once);

            _dbContextMock.Verify(x => x.SaveChangesAsync(), Times.Once);

        }

    }
}
