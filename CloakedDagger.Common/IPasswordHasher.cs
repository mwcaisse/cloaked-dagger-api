namespace CloakedDagger.Common
{
    public interface IPasswordHasher
    {

        /// <summary>
        /// Hashes the given password
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        string HashPassword(string password);

        /// <summary>
        /// Verifies that the given password matches the password hash
        /// </summary>
        /// <param name="hash"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        bool VerifyPassword(string hash, string password);
    }
}