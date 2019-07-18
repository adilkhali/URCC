using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Configuration;
using United_Remote_Coding_Challeng;
using UnitedRemote.Core.Helpers;
using UnitedRemote.Core.Models.V1;
using UnitedRemote.Core.Repositories;
using UnitedRemote.Core.Repositories.Interfaces;
using UnitedRemote.Web.Controllers.V1;
using Xunit;
using UnitedRemote.Core.ViewModels;

namespace UnitedRemote.Web.Test
{
    public class AccountControllerTests
    {
        private IUsersRepository Repository { get; set; }
        private AccountController Controller { get; }

        public AccountControllerTests()
        {

        }


        [Fact]
        public void Register_User_ShouldDoSomething()
        {
            //Arrange
            var testUser = new RegisterViewModel
            {
                Firstname = "Test",
                Lastname = "USer",
                Email = "Test@t.com",
                Password = "Test.123456"
            };
            //Act
            var response = Controller.Register(testUser);

            //Assert
            Assert.NotNull(response);
            Assert.Equal(response.Result, $"{testUser.Firstname} {testUser.Lastname}");
        }
    }
}
