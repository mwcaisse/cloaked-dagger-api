namespace CloakedDagger.Common.ViewModels
{
    public class EmailViewModel
    {
        public EmailAddressViewModel To { get; set; }
        
        public EmailAddressViewModel From { get; set; }
        
        public string Subject { get; set; }
        
        public string Body { get; set; }
    }
}

