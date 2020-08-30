using System;
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

                var users = new List<UserEntity>();
                
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

                userRepositoryMock.Setup(ur => ur.Create(It.IsAny<UserEntity>())).Callback((UserEntity u) =>
                {
                    users.Add(u);
                });
                
                var subject = new UserService(userRepositoryMock.Object, passwordHasherMock.Object);
                
                subject.Register(userRegistration);
                
                userRepositoryMock.Verify(ur => ur.Create(It.IsAny<UserEntity>()), Times.Once);

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

                var users = new List<UserEntity>();
                
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

                userRepositoryMock.Verify(ur => ur.Create(It.IsAny<UserEntity>()), Times.Never);
                
                Assert.NotNull(ex);
                Assert.Contains("Username is not available", ex.Message);
            }
            
            [Fact]
            public void CannotRegisterWithInvalidInfo()
            {
                var userRepositoryMock = new Mock<IUserRepository>();
                var passwordHasherMock = new Mock<IPasswordHasher>();

                var users = new List<UserEntity>();
                
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

                userRepositoryMock.Verify(ur => ur.Create(It.IsAny<UserEntity>()), Times.Never);
                
                Assert.NotNull(ex);
                Assert.NotEmpty(ex.ValidationResults);
            }
        }

        public class GetByIdTests
        {
            [Fact]
            public void TestCanGetExistingUser()
            {
                var userRepositoryMock = new Mock<IUserRepository>();
                var passwordHasherMock = new Mock<IPasswordHasher>();

                var userId = Guid.NewGuid();
                var user = new UserEntity()
                {
                    UserId = userId,
                    Password = "amazinglySecurePassword",
                    Username = "mitchell",
                    Email = "mitchell@mitchell.com"
                };

                userRepositoryMock.Setup(ur => ur.Get(userId)).Returns(user);
                
                var subject = new UserService(userRepositoryMock.Object, passwordHasherMock.Object);

                var fetchedUser = subject.Get(userId);
                Assert.NotNull(fetchedUser);
                Assert.IsNotType<UserEntity>(fetchedUser);
                
                Assert.Equal(user.UserId, fetchedUser.UserId);
                Assert.Equal(user.Username, fetchedUser.Username);
                Assert.Equal(user.Name, fetchedUser.Name);
            }
            
            [Fact]
            public void TestNonExistingUserReturnsNull()
            {
                var userRepositoryMock = new Mock<IUserRepository>();
                var passwordHasherMock = new Mock<IPasswordHasher>();

                var userId = Guid.NewGuid();

                userRepositoryMock.Setup(ur => ur.Get(userId)).Returns(default(UserEntity));
                
                var subject = new UserService(userRepositoryMock.Object, passwordHasherMock.Object);

                var fetchedUser = subject.Get(userId);
                Assert.Null(fetchedUser);
            }
        }
        
    }
}