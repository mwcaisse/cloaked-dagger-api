namespace CloakedDagger.Web.Configuration
{
    public class AuthenticationConfiguration
    {
        public string LoginUrl { get; set; }
        
        public string LogoutUrl { get; set; }
        
        public string CookieName { get; set; }
        
        public string Key { get; set; }
        
        public string KeyPassword { get; set; }
    }
}