using System;

namespace CloakedDagger.Common.ViewModels
{
    public class ClientViewModel
    {
        public Guid ClientId { get; set; }
        
        public string Name { get; set; }
        
        public string Description { get; set; }
    }
}