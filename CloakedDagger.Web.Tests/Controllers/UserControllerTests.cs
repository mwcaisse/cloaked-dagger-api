using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using CloakedDagger.Web.Tests.Fixtures;
using Microsoft.AspNetCore.Mvc.Formatters;
using Newtonsoft.Json;
using Serilog;
using Serilog.Events;
using Xunit;
using Xunit.Abstractions;

namespace CloakedDagger.Web.Tests.Controllers
{
    public class UserControllerTests : IClassFixture<ServiceMockWebTestFixture>
    {
        
        private HttpClient Client { get; }

        private readonly ServiceMockWebTestFixture _factory;

        public UserControllerTests(ServiceMockWebTestFixture factory, ITestOutputHelper outputHelper)
        {
            Log.Logger = new LoggerConfiguration().MinimumLevel.Debug()
                .WriteTo.TestOutput(outputHelper, LogEventLevel.Debug)
                .CreateLogger();
            
            this._factory = factory;
            this.Client = factory.CreateClient();
        }

        [Fact]
        public async Task TestLoginReturnsUnauthorizedWhenLoginFails()
        {
            var username = "mitchell";
            var password = "password";
            
            _factory.MockLoginService.Setup(ls => ls.Login(username, password))
                .Returns(default(ClaimsPrincipal));

            var login = new
            {
                username,
                password
            };
            var content = new StringContent(JsonConvert.SerializeObject(login), Encoding.UTF8, "application/json");

            var resp = await Client.PostAsync("/user/login", content);
            
            Assert.Equal(HttpStatusCode.Unauthorized, resp.StatusCode);

            var respContent = await resp.Content.ReadAsStringAsync();
            Assert.Equal("Invalid credentials provided.", respContent);

        }

    }
}