using System;
using System.Collections.Generic;

namespace CloakedDagger.Common.ViewModels
{
    public class UserViewModel
    {
        public Guid Id { get; set; }
        
        public string Username { get; set; }
        
        public string Name { get; set; }

        public ICollection<string> Roles { get; set; }
        
        public UserLoggedInAdditionalActionViewModel AdditionalActions { get; set; }
    }
}