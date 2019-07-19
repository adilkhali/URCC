using Xunit;
using UnitedRemote.Core.ViewModels;
using System.Net.Http;
using United_Remote_Coding_Challeng;
using System.Net;
using System.Threading.Tasks;
using Moq;
using Microsoft.AspNetCore.Identity;
using UnitedRemote.Core.Models.V1;
using UnitedRemote.Core.Helpers;
using UnitedRemote.Core.Repositories.Interfaces;
using Microsoft.Extensions.Configuration;
using UnitedRemote.Web.Controllers.V1;
using UnitedRemote.Core.Repositories;
using UnitedRemote.Core.ViewModels.Authentication;
using System;

namespace UnitedRemote.Web.Test
{
    public class AccountControllerTests
    {
        private readonly AccountController _accountController;

        public AccountControllerTests()
        {
            var userManager = new FakeUserManager();
            var signInManager = new FakeSignInManager();
            var usersRepository = new UsersRepository(userManager);
            IConfiguration configuration = FakeConfiguration.GetIConfiguration();
            var errorHandler = new Mock<IErrorHandler>();

            _accountController = new AccountController(usersRepository, signInManager, configuration, errorHandler.Object);
        }


        [Fact]
        public async Task Register_User_ShouldResponseWithOK()
        {
            //Arrange
            var TestUser = new RegisterViewModel
            {
                Firstname = "Test",
                Lastname = "USer",
                Email = "Test@t.com",
                Password = "Test.123456"
            };
            var expectedResult = $"{TestUser.Firstname} {TestUser.Lastname}";

            //Act
            var response = await _accountController.Register(TestUser);

            // Assert
            Assert.IsType<string>(response.FullName);
            Assert.Equal(expectedResult, response.FullName);
        }

        [Fact]
        public async Task Login_User_ShouldResponseWithExceptionAsync()
        {
            //Arrange
            var TestUser = new LoginViewModel
            {
                Email = "Test@t.com",
                Password = "Test123456"
            };

            // Act & Assert
            await Assert.ThrowsAsync<HttpRequestException>(() => _accountController.Login(TestUser));
        }

    }

}
