using System;

namespace CloakedDagger.Common.Domain.Events
{
    public abstract class DomainEvent
    {
        public DateTime OccuredOn { get; set; }
        
        public abstract string Type { get; }
    }
}