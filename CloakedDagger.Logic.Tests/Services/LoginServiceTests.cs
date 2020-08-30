using System;
using System.Security.Claims;
using CloakedDagger.Common;
using CloakedDagger.Common.Entities;
using CloakedDagger.Common.Repositories;
using CloakedDagger.Logic.Services;
using Moq;
using Xunit;

namespace CloakedDagger.Logic.Tests.Services
{
    public class LoginServiceTests
    {

        public class LoginTests
        {

            [Fact]
            public void TestWeCanLogin()
            {
                var username = "test";
                var password = "correctPASSWORD";
                var userId = Guid.NewGuid();
                
                var userRepository = new Mock<IUserRepository>();
                var passwordHasher = new Mock<IPasswordHasher>();

                passwordHasher.Setup(ph => 
                    ph.VerifyPassword("correctHASH", password)).Returns(true);

                userRepository.Setup(ur => ur.Get(username)).Returns(new UserEntity()
                {
                    UserId = userId,
                    Username = username,
                    Password = "correctHASH"
                });
                
                var subject = new LoginService(userRepository.Object, passwordHasher.Object);
                var principal = subject.Login(username, password);
                Assert.NotNull(principal);
                
                Assert.Equal("login", principal.Identity.AuthenticationType);
                Assert.True(principal.Identity.IsAuthenticated);

                Assert.Contains(principal.Claims, c => c.Type == ClaimTypes.Name && c.Value == username);
                Assert.Contains(principal.Claims, c => c.Type == ClaimTypes.Sid && c.Value == userId.ToString());
                
                passwordHasher.Verify(ph => ph.VerifyPassword("correctHASH", password), Times.Once);
            }

            [Fact]
            public void TestNonExistentUserReturnsNull()
            {
                var username = "test";
                var password = "doesntReallyMatter";
                
                var userRepository = new Mock<IUserRepository>();
                var passwordHasher = new Mock<IPasswordHasher>();

                userRepository.Setup(ur => ur.Get(username)).Returns(default(UserEntity));
                
                var subject = new LoginService(userRepository.Object, passwordHasher.Object);
                
                Assert.Null(subject.Login(username, password));
                // The password hasher should still be called regardless
                passwordHasher.Verify(ph => ph.VerifyPassword(It.IsAny<string>(), password), Times.Once);
            }

            [Fact]
            public void TestInvalidLoginReturnsNull()
            {
                var username = "test";
                var password = "doesntReallyMatter";
                
                var userRepository = new Mock<IUserRepository>();
                var passwordHasher = new Mock<IPasswordHasher>();

                passwordHasher.Setup(ph => 
                    ph.VerifyPassword(It.IsAny<string>(), It.IsAny<string>())).Returns(false);

                userRepository.Setup(ur => ur.Get(username)).Returns(new UserEntity()
                {
                    Username = username,
                    Password = "amazingHash"
                });
                
                var subject = new LoginService(userRepository.Object, passwordHasher.Object);
                
                Assert.Null(subject.Login(username, password));
                passwordHasher.Verify(ph => ph.VerifyPassword("amazingHash", password), Times.Once);
            }
            
        }
    }
}