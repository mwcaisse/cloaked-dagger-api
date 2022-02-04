using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CloakedDagger.Common;
using CloakedDagger.Common.Entities;
using CloakedDagger.Common.Exceptions;
using CloakedDagger.Common.Repositories;
using CloakedDagger.Common.Services;
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
            public async Task CanRegisterValidUser()
            {
                var userRepositoryMock = new Mock<IUserRepository>();
                var roleRepositoryMock = new Mock<IRoleRepository>();
                var userRoleRepositoryMock = new Mock<IUserRoleRepository>();
                var passwordHasherMock = new Mock<IPasswordHasher>();
                var userRegistrationKeyServiceMock = new Mock<IUserRegistrationKeyService>();
                var userEmailVerificationRequestRepositoryMock = new Mock<IUserEmailVerificationRequestRepository>();
                var emailServiceMock = new Mock<IEmailService>();

                var users = new List<UserEntity>();
                
                var username = "mitchell";
                var password = "securePassword1";
                var passwordHash = "hash_securePassword1";
                
                var userRegistration = new UserRegistrationViewModel()
                {
                    Email = "mitchell@mitchell.com",
                    Username = username,
                    Name = "Mitchell",
                    Password = password,
                    RegistrationKey = Guid.NewGuid().ToString()
                };

                passwordHasherMock.Setup(ph => ph.HashPassword(password)).Returns(passwordHash);
                userRepositoryMock.Setup(ur => ur.UsernameExists(username)).Returns(false);
                userRepositoryMock.Setup(ur => ur.Get(It.IsAny<Guid>())).Returns(new UserEntity());
                roleRepositoryMock.Setup(rr => rr.Get(It.IsAny<Guid>())).Returns(new RoleEntity());

                userRegistrationKeyServiceMock.Setup(urks => urks.IsValid(It.IsAny<string>())).Returns(true);
                userRegistrationKeyServiceMock.Setup(urks => urks.Use(It.IsAny<string>(), It.IsAny<Guid>())).Returns(true);
                
                userRepositoryMock.Setup(ur => ur.Create(It.IsAny<UserEntity>())).Callback((UserEntity u) =>
                {
                    users.Add(u);
                });

                emailServiceMock.Setup(x => x.SendEmailVerificationEmail(It.IsAny<UserEntity>(), It.IsAny<string>()))
                    .ReturnsAsync(true);

                var subject = new UserService(userRepositoryMock.Object, roleRepositoryMock.Object, 
                    userRoleRepositoryMock.Object, passwordHasherMock.Object, userRegistrationKeyServiceMock.Object,
                    userEmailVerificationRequestRepositoryMock.Object, emailServiceMock.Object);
                
                await subject.Register(userRegistration);
                
                userRepositoryMock.Verify(ur => ur.Create(It.IsAny<UserEntity>()), Times.Once);
                
                userEmailVerificationRequestRepositoryMock.Verify(x => 
                    x.Create(It.IsAny<UserEmailVerificationRequestEntity>()), Times.Once);

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
            public async Task CannotRegisterUserIfUsernameIsTaken()
            {
                var userRepositoryMock = new Mock<IUserRepository>();
                var roleRepositoryMock = new Mock<IRoleRepository>();
                var userRoleRepositoryMock = new Mock<IUserRoleRepository>();
                var passwordHasherMock = new Mock<IPasswordHasher>();
                var userRegistrationKeyServiceMock = new Mock<IUserRegistrationKeyService>();
                var userEmailVerificationRequestRepositoryMock = new Mock<IUserEmailVerificationRequestRepository>();
                var emailServiceMock = new Mock<IEmailService>();

                var users = new List<UserEntity>();
                
                var username = "mitchell";
                var password = "securePassword1";
                var passwordHash = "hash_securePassword1";
                
                var userRegistration = new UserRegistrationViewModel()
                {
                    Email = "mitchell@mitchell.com",
                    Username = username,
                    Name = "Mitchell",
                    Password = password,
                    RegistrationKey = "hello world"
                };

                passwordHasherMock.Setup(ph => ph.HashPassword(password)).Returns(passwordHash);
                userRepositoryMock.Setup(ur => ur.UsernameExists(username)).Returns(true);
                
                userRegistrationKeyServiceMock.Setup(urks => urks.IsValid(It.IsAny<string>())).Returns(true);
                userRegistrationKeyServiceMock.Setup(urks => urks.Use(It.IsAny<string>(), It.IsAny<Guid>())).Returns(true);

                
                var subject = new UserService(userRepositoryMock.Object, roleRepositoryMock.Object, 
                    userRoleRepositoryMock.Object, passwordHasherMock.Object, userRegistrationKeyServiceMock.Object,
                    userEmailVerificationRequestRepositoryMock.Object, emailServiceMock.Object);

                var ex = await Assert.ThrowsAsync<EntityValidationException>(() => subject.Register(userRegistration));

                userRepositoryMock.Verify(ur => ur.Create(It.IsAny<UserEntity>()), Times.Never);
                
                Assert.NotNull(ex);
                Assert.Contains("Username is not available", ex.Message);
            }
            
            [Fact]
            public async Task CannotRegisterUserWithoutRegistrationKey()
            {
                var userRepositoryMock = new Mock<IUserRepository>();
                var roleRepositoryMock = new Mock<IRoleRepository>();
                var userRoleRepositoryMock = new Mock<IUserRoleRepository>();
                var passwordHasherMock = new Mock<IPasswordHasher>();
                var userRegistrationKeyServiceMock = new Mock<IUserRegistrationKeyService>();
                var userEmailVerificationRequestRepositoryMock = new Mock<IUserEmailVerificationRequestRepository>();
                var emailServiceMock = new Mock<IEmailService>();

                var users = new List<UserEntity>();
                
                var username = "mitchell";
                var password = "securePassword1";
                var passwordHash = "hash_securePassword1";
                
                var userRegistration = new UserRegistrationViewModel()
                {
                    Email = "mitchell@mitchell.com",
                    Username = username,
                    Name = "Mitchell",
                    Password = password,
                    RegistrationKey = null
                };

                passwordHasherMock.Setup(ph => ph.HashPassword(password)).Returns(passwordHash);
                userRepositoryMock.Setup(ur => ur.UsernameExists(username)).Returns(false);
                userRepositoryMock.Setup(ur => ur.Get(It.IsAny<Guid>())).Returns(new UserEntity());
                roleRepositoryMock.Setup(rr => rr.Get(It.IsAny<Guid>())).Returns(new RoleEntity());

                userRegistrationKeyServiceMock.Setup(urks => urks.IsValid(It.IsAny<string>())).Returns(true);
                userRegistrationKeyServiceMock.Setup(urks => urks.Use(It.IsAny<string>(), It.IsAny<Guid>())).Returns(true);
                
                userRepositoryMock.Setup(ur => ur.Create(It.IsAny<UserEntity>())).Callback((UserEntity u) =>
                {
                    users.Add(u);
                });

                var subject = new UserService(userRepositoryMock.Object, roleRepositoryMock.Object, 
                    userRoleRepositoryMock.Object, passwordHasherMock.Object, userRegistrationKeyServiceMock.Object,
                    userEmailVerificationRequestRepositoryMock.Object, emailServiceMock.Object);
                
                var ex = await Assert.ThrowsAsync<EntityValidationException>(() => subject.Register(userRegistration));

                userRepositoryMock.Verify(ur => ur.Create(It.IsAny<UserEntity>()), Times.Never);
                
                Assert.NotNull(ex);
                Assert.NotEmpty(ex.ValidationResults);
            }
            
            [Fact]
            public async Task CannotRegisterUserWithInvalidRegistrationKey()
            {
                var userRepositoryMock = new Mock<IUserRepository>();
                var roleRepositoryMock = new Mock<IRoleRepository>();
                var userRoleRepositoryMock = new Mock<IUserRoleRepository>();
                var passwordHasherMock = new Mock<IPasswordHasher>();
                var userRegistrationKeyServiceMock = new Mock<IUserRegistrationKeyService>();
                var userEmailVerificationRequestRepositoryMock = new Mock<IUserEmailVerificationRequestRepository>();
                var emailServiceMock = new Mock<IEmailService>();

                var users = new List<UserEntity>();
                
                var username = "mitchell";
                var password = "securePassword1";
                var passwordHash = "hash_securePassword1";
                
                var userRegistration = new UserRegistrationViewModel()
                {
                    Email = "mitchell@mitchell.com",
                    Username = username,
                    Name = "Mitchell",
                    Password = password,
                    RegistrationKey = "hello world"
                };

                passwordHasherMock.Setup(ph => ph.HashPassword(password)).Returns(passwordHash);
                userRepositoryMock.Setup(ur => ur.UsernameExists(username)).Returns(false);
                userRepositoryMock.Setup(ur => ur.Get(It.IsAny<Guid>())).Returns(new UserEntity());
                roleRepositoryMock.Setup(rr => rr.Get(It.IsAny<Guid>())).Returns(new RoleEntity());

                userRegistrationKeyServiceMock.Setup(urks => urks.IsValid(It.IsAny<string>())).Returns(false);
                userRegistrationKeyServiceMock.Setup(urks => urks.Use(It.IsAny<string>(), It.IsAny<Guid>())).Returns(false);
                
                userRepositoryMock.Setup(ur => ur.Create(It.IsAny<UserEntity>())).Callback((UserEntity u) =>
                {
                    users.Add(u);
                });

                var subject = new UserService(userRepositoryMock.Object, roleRepositoryMock.Object, 
                    userRoleRepositoryMock.Object, passwordHasherMock.Object, userRegistrationKeyServiceMock.Object,
                    userEmailVerificationRequestRepositoryMock.Object, emailServiceMock.Object);
         
                var ex = await Assert.ThrowsAsync<EntityValidationException>(() => subject.Register(userRegistration));

                userRepositoryMock.Verify(ur => ur.Create(It.IsAny<UserEntity>()), Times.Never);
                
                Assert.NotNull(ex);
                Assert.Contains("Registration Key is not valid!", ex.Message);
            }
            
            [Fact]
            public async Task CannotRegisterWithInvalidInfo()
            {
                var userRepositoryMock = new Mock<IUserRepository>();
                var roleRepositoryMock = new Mock<IRoleRepository>();
                var userRoleRepositoryMock = new Mock<IUserRoleRepository>();
                var passwordHasherMock = new Mock<IPasswordHasher>();
                var userRegistrationKeyServiceMock = new Mock<IUserRegistrationKeyService>();
                var userEmailVerificationRequestRepositoryMock = new Mock<IUserEmailVerificationRequestRepository>();
                var emailServiceMock = new Mock<IEmailService>();

                var users = new List<UserEntity>();
                
                var username = "mitchell";

                var userRegistration = new UserRegistrationViewModel()
                {
                    Email = "",
                    Username = username,
                    Name = "",
                    Password = "",
                    RegistrationKey = "hello world"
                };

                userRepositoryMock.Setup(ur => ur.UsernameExists(username)).Returns(true);
                
                userRegistrationKeyServiceMock.Setup(urks => urks.IsValid(It.IsAny<string>())).Returns(true);
                userRegistrationKeyServiceMock.Setup(urks => urks.Use(It.IsAny<string>(), It.IsAny<Guid>())).Returns(true);

                
                var subject = new UserService(userRepositoryMock.Object, roleRepositoryMock.Object, 
                    userRoleRepositoryMock.Object, passwordHasherMock.Object, userRegistrationKeyServiceMock.Object,
                    userEmailVerificationRequestRepositoryMock.Object, emailServiceMock.Object);

                var ex = await Assert.ThrowsAsync<EntityValidationException>(() => subject.Register(userRegistration));

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
                var roleRepositoryMock = new Mock<IRoleRepository>();
                var userRoleRepositoryMock = new Mock<IUserRoleRepository>();
                var passwordHasherMock = new Mock<IPasswordHasher>();
                var userRegistrationKeyServiceMock = new Mock<IUserRegistrationKeyService>();
                var userEmailVerificationRequestRepositoryMock = new Mock<IUserEmailVerificationRequestRepository>();
                var emailServiceMock = new Mock<IEmailService>();

                var userId = Guid.NewGuid();
                var user = new UserEntity()
                {
                    UserId = userId,
                    Password = "amazinglySecurePassword",
                    Username = "mitchell",
                    Email = "mitchell@mitchell.com",
                    Roles = new List<UserRoleEntity>(),
                };

                userRepositoryMock.Setup(ur => ur.Get(userId)).Returns(user);
                
                var subject = new UserService(userRepositoryMock.Object, roleRepositoryMock.Object, 
                    userRoleRepositoryMock.Object, passwordHasherMock.Object, userRegistrationKeyServiceMock.Object,
                    userEmailVerificationRequestRepositoryMock.Object, emailServiceMock.Object);
                
                var fetchedUser = subject.Get(userId);
                Assert.NotNull(fetchedUser);
                Assert.IsNotType<UserEntity>(fetchedUser);
                
                Assert.Equal(user.UserId, fetchedUser.Id);
                Assert.Equal(user.Username, fetchedUser.Username);
                Assert.Equal(user.Name, fetchedUser.Name);
            }
            
            [Fact]
            public void TestNonExistingUserReturnsNull()
            {
                var userRepositoryMock = new Mock<IUserRepository>();
                var roleRepositoryMock = new Mock<IRoleRepository>();
                var userRoleRepositoryMock = new Mock<IUserRoleRepository>();
                var passwordHasherMock = new Mock<IPasswordHasher>();
                var userRegistrationKeyServiceMock = new Mock<IUserRegistrationKeyService>();
                var userEmailVerificationRequestRepositoryMock = new Mock<IUserEmailVerificationRequestRepository>();
                var emailServiceMock = new Mock<IEmailService>();

                var userId = Guid.NewGuid();

                userRepositoryMock.Setup(ur => ur.Get(userId)).Returns(default(UserEntity));
             
                var subject = new UserService(userRepositoryMock.Object, roleRepositoryMock.Object, 
                    userRoleRepositoryMock.Object, passwordHasherMock.Object, userRegistrationKeyServiceMock.Object,
                    userEmailVerificationRequestRepositoryMock.Object, emailServiceMock.Object);

                var fetchedUser = subject.Get(userId);
                Assert.Null(fetchedUser);
            }
        }
        
    }
}