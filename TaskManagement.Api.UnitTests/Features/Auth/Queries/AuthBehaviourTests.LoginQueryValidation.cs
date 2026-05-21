using AutoFixture;
using MediatR;
using Moq;
using Shouldly;
using TaskManagement.Application.Exceptions;
using TaskManagement.Application.Features.Auth.Queries.Common;
using TaskManagement.Application.Features.Auth.Queries.Login;
using TaskManagement.Domain.Entities;

namespace TaskManagement.Api.UnitTests.Features.Auth
{
    public partial class AuthHandlersTests
    {
        [Fact]
        public async Task ValidateUserBehaviour_WithValidCommand_ShouldProvideSuccessValidation()
        {
            var loginQueryValidationBehaviour = _fixture.Build<LoginQueryValidationBehaviour>().Create();


            var returnedUser = _fixture.Create<UserEntity>();

            var loginQuery = _fixture.Build<LoginQuery>()
                .With(x => x.Email, returnedUser.Email)
                .With(x => x.Password, returnedUser.Password)
                .Create();

            var nextMock = new Mock<RequestHandlerDelegate<AuthQueryResponse>>();

            SetupValidateUserBehaviourTests(returnedUser, loginQuery);

            var result = await loginQueryValidationBehaviour.Handle(loginQuery, nextMock.Object, default);

            result.ShouldBeNull();

            _userRepositoryMock.Verify(x => x.GetUserByEmailAsync(It.Is<string>(x => x == loginQuery.Email)), Times.Once);
            _passwordHasherMock.Verify(x => x.HashAsync(loginQuery.Password), Times.Once);

        }

        [Fact]
        public async Task ValidateUserBehaviour_WithNonExistentEmail_ShouldThrowException()
        {
            var loginQueryValidationBehaviour = _fixture.Build<LoginQueryValidationBehaviour>().Create();

            var loginQuery = _fixture.Build<LoginQuery>().Create();

            var nextMock = new Mock<RequestHandlerDelegate<AuthQueryResponse>>();

            var returnedUser = _fixture.Create<UserEntity>();

            SetupValidateUserBehaviourTests(returnedUser, loginQuery);

            var handlerCall = async () => await loginQueryValidationBehaviour.Handle(loginQuery, nextMock.Object, default);

            handlerCall.ShouldThrow<BadRequestException>();

            _userRepositoryMock.Verify(x => x.GetUserByEmailAsync(It.Is<string>(x => x == loginQuery.Email)), Times.Once);
            _passwordHasherMock.Verify(x => x.HashAsync(loginQuery.Password), Times.Never);

        }


        [Fact]
        public async Task ValidateUserBehaviour_WithInvalidPassword_ShouldThrowException()
        {
            var loginQueryValidationBehaviour = _fixture.Build<LoginQueryValidationBehaviour>().Create();

            var returnedUser = _fixture.Create<UserEntity>();

            var loginQuery = _fixture.Build<LoginQuery>().With(x => x.Email, returnedUser.Email).Create();

            var nextMock = new Mock<RequestHandlerDelegate<AuthQueryResponse>>();

            SetupValidateUserBehaviourTests(returnedUser, loginQuery);

            var handlerCall = async () => await loginQueryValidationBehaviour.Handle(loginQuery, nextMock.Object, default);

            handlerCall.ShouldThrow<BadRequestException>();

            _userRepositoryMock.Verify(x => x.GetUserByEmailAsync(It.Is<string>(x => x == loginQuery.Email)), Times.Once);
            _passwordHasherMock.Verify(x => x.HashAsync(loginQuery.Password), Times.Once);

        }

        private void SetupValidateUserBehaviourTests(UserEntity returnedUser, LoginQuery loginQuery)
        {
            _userRepositoryMock.Setup(x => x.GetUserByEmailAsync(returnedUser.Email)).ReturnsAsync(returnedUser);

            _passwordHasherMock.Setup(x => x.HashAsync(loginQuery.Password)).ReturnsAsync(loginQuery.Password);
        }
    }
}
