using CloakedDagger.Logic.PasswordHasher;
using Xunit;

namespace CloakedDagger.Logic.Tests.PasswordHasher
{
    public class ArgonPasswordHasherTests
    {
        public class HashPasswordTests
        {

            [Fact]
            public void HashedPasswordIsDifferent()
            {
                var hasher = new ArgonPasswordHasher();

                var password = "hello_world";
                Assert.NotEqual(password, hasher.HashPassword(password));
            }

        }

        public class VerifyPasswordTests
        {
            [Fact]
            public void VerifyingCorrectPasswordIsTrue()
            {
                var password = "myAmazingPASSword";
                var hasher = new ArgonPasswordHasher();

                var hashedPassword = hasher.HashPassword(password);
                Assert.True(hasher.VerifyPassword(hashedPassword, password));
            }

            [Fact]
            public void VerifyingIncorrectPasswordIsFalse()
            {
                var password = "myAmazingPASSword";
                var hasher = new ArgonPasswordHasher();

                var hashedPassword = hasher.HashPassword(password);
                Assert.False(hasher.VerifyPassword(hashedPassword, "SPARTA"));
            }
            
            [Fact]
            public void VerifyingPasswordIsCaseSensitive()
            {
                var password = "myAmazingPASSword";
                var hasher = new ArgonPasswordHasher();

                var hashedPassword = hasher.HashPassword(password);
                Assert.False(hasher.VerifyPassword(hashedPassword, password.ToLower()));
                Assert.False(hasher.VerifyPassword(hashedPassword, password.ToUpper()));
            }
        }
    }
}