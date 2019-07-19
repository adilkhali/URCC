using System.Net.Http;
using Microsoft.AspNetCore.TestHost;
using Microsoft.AspNetCore.Hosting;
using United_Remote_Coding_Challeng;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Identity;
using UnitedRemote.Core.Models.V1;
using Moq;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authentication;
using System;
using System.Threading.Tasks;
using System.Security.Claims;

namespace UnitedRemote.Web.Test
{

    public class FakeSignInManager : SignInManager<ApplicationUser>
    {
        public FakeSignInManager()
                : base(new Mock<FakeUserManager>().Object,
                     new HttpContextAccessor() { HttpContext = new DefaultHttpContext() { RequestServices = createServiceProviderMock() } },
                     new Mock<IUserClaimsPrincipalFactory<ApplicationUser>>().Object,
                     new Mock<IOptions<IdentityOptions>>().Object,
                     new Mock<ILogger<SignInManager<ApplicationUser>>>().Object,
                     new Mock<IAuthenticationSchemeProvider>().Object)
        { }

        public static IServiceProvider createServiceProviderMock()
        {
            var authServiceMock = new Mock<IAuthenticationService>();
            authServiceMock
                .Setup(_ => _.SignInAsync(It.IsAny<HttpContext>(), It.IsAny<string>(), It.IsAny<ClaimsPrincipal>(), It.IsAny<AuthenticationProperties>()))
                .Returns(Task.FromResult((object)null)); //<-- to allow async call to continue

            var serviceProviderMock = new Mock<IServiceProvider>();
            serviceProviderMock
                .Setup(_ => _.GetService(typeof(IAuthenticationService)))
                .Returns(authServiceMock.Object);

            return serviceProviderMock.Object;
        }
    }

    public class FakeUserManager : UserManager<ApplicationUser>
    {
        public FakeUserManager()
            : base(new Mock<IUserStore<ApplicationUser>>().Object,
              new Mock<IOptions<IdentityOptions>>().Object,
              new Mock<IPasswordHasher<ApplicationUser>>().Object,
              new IUserValidator<ApplicationUser>[0],
              new IPasswordValidator<ApplicationUser>[0],
              new Mock<ILookupNormalizer>().Object,
              new Mock<IdentityErrorDescriber>().Object,
              new Mock<IServiceProvider>().Object,
              new Mock<ILogger<UserManager<ApplicationUser>>>().Object)
        { }

        public override Task<IdentityResult> CreateAsync(ApplicationUser user, string password)
        {
            return Task.FromResult(IdentityResult.Success);
        }

        public override Task<IdentityResult> AddToRoleAsync(ApplicationUser user, string role)
        {
            return Task.FromResult(IdentityResult.Success);
        }

        public override Task<string> GenerateEmailConfirmationTokenAsync(ApplicationUser user)
        {
            return Task.FromResult(Guid.NewGuid().ToString());
        }

    }

    public static class FakeConfiguration{
        public static IConfiguration GetIConfiguration()
        {
            return new ConfigurationBuilder()
                   .AddJsonFile("appsettings.json")
                   .AddEnvironmentVariables()
                   .Build();
        }
    }

}
