using System;
using System.Collections.Generic;

namespace CloakedDagger.Common.ViewModels
{
    public class UserViewModel
    {
        public Guid UserId { get; set; }
        
        public string Username { get; set; }
        
        public string Name { get; set; }

        public ICollection<string> Roles { get; set; }
    }
}