using System.Collections.Generic;
using System.Linq;
using CloakedDagger.Common;
using CloakedDagger.Common.Entities;
using CloakedDagger.Common.Exceptions;
using CloakedDagger.Common.Repositories;
using CloakedDagger.Common.ViewModels;
using CloakedDagger.Logic.Services;
using Moq;
using Xunit;

namespace CloakedDagger.Logic.Tests.Services
{
    public class UserServiceTests
    {

        public class RegisterTests
        {
            [Fact]
            public void CanRegisterValidUser()
            {
                var userRepositoryMock = new Mock<IUserRepository>();
                var passwordHasherMock = new Mock<IPasswordHasher>();

                var users = new List<User>();
                
                var username = "mitchell";
                var password = "securePassword1";
                var passwordHash = "hash_securePassword1";
                
                var userRegistration = new UserRegistrationViewModel()
                {
                    Email = "mitchell@mitchell.com",
                    Username = username,
                    Name = "Mitchell",
                    Password = password
                };

                passwordHasherMock.Setup(ph => ph.HashPassword(password)).Returns(passwordHash);
                userRepositoryMock.Setup(ur => ur.UsernameExists(username)).Returns(false);

                userRepositoryMock.Setup(ur => ur.Create(It.IsAny<User>())).Callback((User u) =>
                {
                    users.Add(u);
                });
                
                var subject = new UserService(userRepositoryMock.Object, passwordHasherMock.Object);
                
                subject.Register(userRegistration);
                
                userRepositoryMock.Verify(ur => ur.Create(It.IsAny<User>()), Times.Once);

                Assert.NotEmpty(users);
                var createdUser = users.First();
                
                Assert.Equal(userRegistration.Email, createdUser.Email);
                Assert.Equal(userRegistration.Name, createdUser.Name);
                Assert.Equal(userRegistration.Username, createdUser.Username);
                Assert.Equal(passwordHash, createdUser.Password);
                Assert.NotEqual(password, createdUser.Password);
                Assert.True(createdUser.Active);
            }

            [Fact]
            public void CannotRegisterUserIfUsernameIsTaken()
            {
                var userRepositoryMock = new Mock<IUserRepository>();
                var passwordHasherMock = new Mock<IPasswordHasher>();

                var users = new List<User>();
                
                var username = "mitchell";
                var password = "securePassword1";
                var passwordHash = "hash_securePassword1";
                
                var userRegistration = new UserRegistrationViewModel()
                {
                    Email = "mitchell@mitchell.com",
                    Username = username,
                    Name = "Mitchell",
                    Password = password
                };

                passwordHasherMock.Setup(ph => ph.HashPassword(password)).Returns(passwordHash);
                userRepositoryMock.Setup(ur => ur.UsernameExists(username)).Returns(true);
                
                var subject = new UserService(userRepositoryMock.Object, passwordHasherMock.Object);

                var ex = Assert.Throws<EntityValidationException>(() => subject.Register(userRegistration));

                userRepositoryMock.Verify(ur => ur.Create(It.IsAny<User>()), Times.Never);
                
                Assert.NotNull(ex);
                Assert.Contains("Username is not available", ex.Message);
            }
            
            [Fact]
            public void CannotRegisterWithInvalidInfo()
            {
                var userRepositoryMock = new Mock<IUserRepository>();
                var passwordHasherMock = new Mock<IPasswordHasher>();

                var users = new List<User>();
                
                var username = "mitchell";

                var userRegistration = new UserRegistrationViewModel()
                {
                    Email = "",
                    Username = username,
                    Name = "",
                    Password = ""
                };

                userRepositoryMock.Setup(ur => ur.UsernameExists(username)).Returns(true);
                
                var subject = new UserService(userRepositoryMock.Object, passwordHasherMock.Object);

                var ex = Assert.Throws<EntityValidationException>(() => subject.Register(userRegistration));

                userRepositoryMock.Verify(ur => ur.Create(It.IsAny<User>()), Times.Never);
                
                Assert.NotNull(ex);
                Assert.NotEmpty(ex.ValidationResults);
            }
        }
        
    }
}