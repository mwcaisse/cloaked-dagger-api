using System.ComponentModel.DataAnnotations;
using System.Linq;
using CloakedDagger.Common.Exceptions;
using Xunit;

namespace CloakedDagger.Logic.Tests.Validation
{
    public class ValidationUtilsTests
    {
        
        public class ValidateViewModelTests : ValidationUtilsTests {

            [Fact]
            public void TestDoesntThrowExceptionWithValidViewModel()
            {
                var vm = new TestViewModel()
                {
                    Name = "Bob",
                    Age = 25,
                    Address = "100 Boston St"
                };
                
                ValidationUtils.ValidateViewModel(vm);
                Assert.True(true);
            }
            
            [Fact]
            public void TestDoesntThrowExceptionWithValidViewModelWithMissingOptionalField()
            {
                var vm = new TestViewModel()
                {
                    Name = "Bob",
                    Age = 25
                };
                
                ValidationUtils.ValidateViewModel(vm);
                Assert.True(true);
            }

            [Fact]
            public void TestThrowsExceptionWithInvalidViewModel()
            {
                var vm = new TestViewModel()
                {
                    Name = "Bob 10101010101010",
                    Age = 25
                };

                var ex = Assert.Throws<EntityValidationException>(() => ValidationUtils.ValidateViewModel(vm));
                Assert.Single(ex.ValidationResults);
                var res = ex.ValidationResults.First();
                
                Assert.Equal("Name must not be move than 10 characters", res.ErrorMessage);
                Assert.Equal("Name", res.MemberNames.First());
            }

            [Fact]
            public void TestThrowsExceptionWithSpecifiedMessageWithInvalidViewModel()
            {
                var vm = new TestViewModel()
                {
                    Name = "Bob 10101010101010",
                    Age = 25
                };

                var errorMessage = "Test View Model is not valid! Please correct any issues and try again!";

                var ex = Assert.Throws<EntityValidationException>(() => 
                    ValidationUtils.ValidateViewModel(vm, errorMessage));
                
                Assert.Equal(errorMessage, ex.Message);
            }
            
        }

        private class TestViewModel
        {
            [Required(ErrorMessage = "Name must be provided")]
            [StringLength(10, ErrorMessage = "Name must not be move than 10 characters")]
            public string Name { get; set; }
            
            [Range(0, 120, ErrorMessage = "Age must be between 0 and 120, inclusive")]
            public int Age { get; set; }
            
            public string Address { get; set; } 
        }
    }
}