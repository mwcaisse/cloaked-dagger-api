using CloakedDagger.Common;
using Sodium;

namespace CloakedDagger.Logic.PasswordHasher
{
    public class ArgonPasswordHasher : IPasswordHasher
    {
        public string HashPassword(string password)
        {
            return PasswordHash.ArgonHashString(password, PasswordHash.StrengthArgon.Moderate);
        }

        public bool VerifyPassword(string hash, string password)
        {
            return PasswordHash.ArgonHashStringVerify(hash, password);
        }
    }
}