using CloakedDagger.Common.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;

namespace CloakedDagger.Web.Tests.Fixtures
{
    public class ServiceMockWebTestFixture : WebApplicationFactory<Startup>
    {

        public readonly Mock<ILoginService> MockLoginService;

        public readonly Mock<IUserService> MockUserService;

        public ServiceMockWebTestFixture()
        {
            MockLoginService = new Mock<ILoginService>();
            MockUserService = new Mock<IUserService>();
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseEnvironment("Testing");

            builder.ConfigureTestServices(services =>
            {
                services.AddLogging(options =>
                {
                    options.AddDebug();
                    options.AddConsole();
                });

                services.AddSingleton(MockLoginService.Object);
                services.AddSingleton(MockUserService.Object);
            });
        }

    }
}