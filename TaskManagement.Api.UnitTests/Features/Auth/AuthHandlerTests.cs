using AutoFixture;
using Microsoft.Extensions.Options;
using Moq;
using System.Reflection;
using TaskManagement.Api.UnitTests.SpecimanBuilders;
using TaskManagement.Application.Tokens;
using TaskManagement.Domain.Entities;
using TaskManagement.Domain.Interfaces;
using TaskManagement.Domain.Interfaces.Users;

namespace TaskManagement.Api.UnitTests.Features.Auth
{
    public partial class AuthHandlersTests
    {
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Fixture _fixture;
        private readonly Mock<ITokenGenerator> _tokenGeneratorMock;
        private readonly Mock<IRefreshTokenRepository> _refreshTokenRepositoryMock;
        private readonly Mock<IOptions<JwtSettings>> _jwtSettingsMock;
        private readonly Mock<IAppDbContext> _dbContextMock;
        private readonly Mock<IPasswordHasher> _passwordHasherMock;
        public AuthHandlersTests()
        {
            _userRepositoryMock = new();
            _refreshTokenRepositoryMock = new();
            _fixture = new();
            _jwtSettingsMock = new();
            _tokenGeneratorMock = new();
            _dbContextMock = new();
            _passwordHasherMock = new();

            _fixture.Inject(_userRepositoryMock.Object);
            _fixture.Inject(_tokenGeneratorMock.Object);
            _fixture.Inject(_refreshTokenRepositoryMock.Object);
            _fixture.Inject(_dbContextMock.Object);
            _fixture.Inject(_passwordHasherMock.Object);

            _fixture.Customizations.Insert(0, new PrivateSetterSpecimenBuilder(typeof(UserEntity), typeof(RefreshTokenEntity)));
        }

    }
}
