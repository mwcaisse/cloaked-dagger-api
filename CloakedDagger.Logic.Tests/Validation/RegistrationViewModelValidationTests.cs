using System.Linq;
using CloakedDagger.Common.Exceptions;
using CloakedDagger.Common.ViewModels;
using Xunit;

namespace CloakedDagger.Logic.Tests.Validation
{
    public class RegistrationViewModelValidationTests
    {

        [Fact]
        public void TestBlankUsernameThrowsError()
        {
            var registration = new UserRegistrationViewModel()
            {
                Username = "",
                Password = "helloworld_long_password",
                Email = "hello@hello.com",
                Name = "hello"
            };

            var ex = Assert.Throws<EntityValidationException>(() => 
                ValidationUtils.ValidateViewModel(registration));

            Assert.Single(ex.ValidationResults);
            var res = ex.ValidationResults.First();
            
            Assert.Equal("Username", res.MemberNames.First());

        }

        [Fact]
        public void TestBlankPasswordThrowsError()
        {
            var registration = new UserRegistrationViewModel()
            {
                Username = "hello",
                Password = "",
                Email = "hello@hello.com",
                Name = "hello"
            };

            var ex = Assert.Throws<EntityValidationException>(() => 
                ValidationUtils.ValidateViewModel(registration));

            Assert.NotEmpty(ex.ValidationResults);

            Assert.Contains(ex.ValidationResults, 
                res => res.MemberNames.Contains("Password") 
                       && res.ErrorMessage.Contains("Password is required"));
        }

        [Fact]
        public void TestBlankEmailThrowsError()
        {
            var registration = new UserRegistrationViewModel()
            {
                Username = "hello",
                Password = "securepassword2",
                Email = "",
                Name = "hello"
            };

            var ex = Assert.Throws<EntityValidationException>(() => 
                ValidationUtils.ValidateViewModel(registration));

            Assert.NotEmpty(ex.ValidationResults);

            Assert.Contains(ex.ValidationResults, 
                res => res.MemberNames.Contains("Email") 
                       && res.ErrorMessage.Contains("Email is required"));
        }

        [Fact]
        public void TestPasswordsMustBeAtleast8Characters()
        {
            var registration = new UserRegistrationViewModel()
            {
                Username = "hello",
                Password = "12345",
                Email = "email@email.com",
                Name = "hello"
            };

            var ex = Assert.Throws<EntityValidationException>(() => 
                ValidationUtils.ValidateViewModel(registration));

            Assert.NotEmpty(ex.ValidationResults);

            Assert.Contains(ex.ValidationResults, 
                res => res.MemberNames.Contains("Password") 
                       && res.ErrorMessage.Contains("Password must be at least 8 characters long"));
        }

        [Fact]
        public void TestValidUserIsAccepted()
        {
            var registration = new UserRegistrationViewModel()
            {
                Name = "Ron Swansom",
                Email = "ron@why_you_need_this.com",
                Password = "securePASSWORD123#",
                Username = "ron"
            };
            
            ValidationUtils.ValidateViewModel(registration);
            Assert.True(true);
        }
        
    }
}