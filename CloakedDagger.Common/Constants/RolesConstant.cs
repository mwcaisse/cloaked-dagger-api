using System;

namespace CloakedDagger.Common.Constants
{
    public static class Roles
    {
        public static class User
        {
            public const string Name = "User"; 
            public static readonly Guid Id = Guid.Parse("0e60c1c0-5934-11eb-9cc6-00cf12e857f9");  
        }
        
        public static class Admin
        {
            public const string Name = "Admin"; 
            public static readonly Guid Id = Guid.Parse("d4a3453f-5935-11eb-9cc6-00cf12e857f9");  
        }
    }
}