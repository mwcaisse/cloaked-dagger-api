using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using CloakedDagger.Common.Constants;
using CloakedDagger.Common.ViewModels;
using CloakedDagger.Web.Tests.Fixtures;
using Moq;
using Newtonsoft.Json;
using Serilog;
using Serilog.Events;
using Xunit;
using Xunit.Abstractions;

namespace CloakedDagger.Web.Tests.Controllers
{
    public class UserControllerTests
    {
        public class LoginTests : IClassFixture<ServiceMockWebTestFixture>
        {
            
            private HttpClient Client { get; }

            private readonly ServiceMockWebTestFixture _factory;

            public LoginTests(ServiceMockWebTestFixture factory, ITestOutputHelper outputHelper)
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

          
                var content = new StringContent(JsonConvert.SerializeObject(new {
                    username,
                    password
                }), Encoding.UTF8, "application/json");

                var resp = await Client.PostAsync("/api/user/login", content);
                
                Assert.Equal(HttpStatusCode.Unauthorized, resp.StatusCode);

                var respContent = await resp.Content.ReadAsStringAsync();
                Assert.Equal("Invalid credentials provided.", respContent);

            }

            [Fact]
            public async Task TestLoginReturnsOkWhenSuccessful()
            {
                var username = "mitchell";
                var password = "password";

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, username),
                    new(ClaimTypes.Sid, username),
                    new(UserClaims.Subject, username),
                    new(UserClaims.EmailVerified, "true")
                    
                };
                var identity = new ClaimsIdentity(claims, "login");
                var principal = new ClaimsPrincipal(identity);
                
                _factory.MockLoginService.Setup(ls => ls.Login(username, password))
                    .Returns(principal);
                
                var content = new StringContent(JsonConvert.SerializeObject(new {
                    username,
                    password
                }), Encoding.UTF8, "application/json");

                var resp = await Client.PostAsync("/api/user/login", content);
                
                Assert.Equal(HttpStatusCode.OK, resp.StatusCode);
            }
            
            [Fact]
            public async Task TestLoginReturns206WhenSuccessfulButRequiresEmailVerification()
            {
                var username = "mitchell";
                var password = "password";

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, username),
                    new(ClaimTypes.Sid, username),
                    new(UserClaims.Subject, username)
                };
                var identity = new ClaimsIdentity(claims, "login");
                var principal = new ClaimsPrincipal(identity);
                
                _factory.MockLoginService.Setup(ls => ls.Login(username, password))
                    .Returns(principal);
                
                var content = new StringContent(JsonConvert.SerializeObject(new {
                    username,
                    password
                }), Encoding.UTF8, "application/json");

                var resp = await Client.PostAsync("/api/user/login", content);
                
                Assert.Equal(HttpStatusCode.PartialContent, resp.StatusCode);
                
                var respContent = await resp.Content.ReadAsStringAsync();
                var addititionalActions = JsonConvert.DeserializeObject<UserLoggedInAdditionalActionViewModel>(respContent);
                
                Assert.NotNull(addititionalActions);
                Assert.True(addititionalActions.EmailVerificationRequired);
            }

        }

        public class MeTests : IClassFixture<ServiceMockAuthenticationWebTestFixture>
        {
            private HttpClient Client { get; }

            private readonly ServiceMockAuthenticationWebTestFixture _factory;

            public MeTests(ServiceMockAuthenticationWebTestFixture factory, ITestOutputHelper outputHelper)
            {
                Log.Logger = new LoggerConfiguration().MinimumLevel.Debug()
                    .WriteTo.TestOutput(outputHelper, LogEventLevel.Debug)
                    .CreateLogger();
                
                this._factory = factory;
                this.Client = factory.CreateClient();
            }

            [Fact]
            public async Task TestMeReturnsUseWhenLoggedIn()
            {
                var userId = Guid.Parse(TestAuthHandler.TestUserId);
                
                _factory.MockLoginService.Setup(us => us.GetUserViewModelFromPrincipal(It.IsAny<ClaimsPrincipal>()))
                    .Returns(new UserViewModel()
                    {
                        Username = TestAuthHandler.TestUsername,
                        Name = TestAuthHandler.TestUsername,
                        Id = userId
                    });

                var resp = await Client.GetAsync("/api/user/me");
                Assert.Equal(HttpStatusCode.OK, resp.StatusCode);

                var respContent = await resp.Content.ReadAsStringAsync();
                var userResponseDict = JsonConvert.DeserializeObject<Dictionary<string, string>>(respContent);
                
                Assert.DoesNotContain("password", userResponseDict.Keys);

                var userResp = JsonConvert.DeserializeObject<UserViewModel>(respContent);
                Assert.Equal(userId, userResp.Id);
                Assert.Equal(TestAuthHandler.TestUsername, userResp.Username);
                Assert.Equal(TestAuthHandler.TestUsername, userResp.Name);

            }
        }
        
        public class MeUnauthenticatedTests : IClassFixture<ServiceMockWebTestFixture>
        {
            private HttpClient Client { get; }

            private readonly ServiceMockWebTestFixture _factory;

            public MeUnauthenticatedTests(ServiceMockWebTestFixture factory, ITestOutputHelper outputHelper)
            {
                Log.Logger = new LoggerConfiguration().MinimumLevel.Debug()
                    .WriteTo.TestOutput(outputHelper, LogEventLevel.Debug)
                    .CreateLogger();
                
                this._factory = factory;
                this.Client = factory.CreateClient();
            }

            [Fact]
            public async Task TestMeReturnsUnauthorizedWhenNotLoggedIn()
            {
                var userId = Guid.Parse(TestAuthHandler.TestUserId);
                
                _factory.MockUserService.Setup(us => us.Get(userId))
                    .Returns(new UserViewModel()
                    {
                        Username = TestAuthHandler.TestUsername,
                        Name = TestAuthHandler.TestUsername,
                        Id = userId
                    });

                var resp = await Client.GetAsync("/api/user/me");
                Assert.Equal(HttpStatusCode.Unauthorized, resp.StatusCode);
            }
        }
    }
}