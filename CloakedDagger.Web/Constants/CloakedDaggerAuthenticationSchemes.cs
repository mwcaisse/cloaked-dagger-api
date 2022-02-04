namespace CloakedDagger.Web.Constants
{
    // Adding CloakedDagger prefix to de-conflict with the many other classes called this
    public static class CloakedDaggerAuthenticationSchemes
    {
        public const string Default = "Cookies";
        // This is used when the user has presented proper credentials, but still have additional actions to take ot be
        // fully authenticated (i.e. email verification or MFA)
        public const string Partial = "Partial";
    }
}