using System;

namespace CloakedDagger.Common.Entities
{
    public class PersistedGrantEntity
    {
        public string Id { get; set; }
        
        public string Type { get; set; }
        
        public string SubjectId { get; set; }
        
        public string SessionId { get; set; }
        
        public string ClientId { get; set; }
        
        public string Description { get; set; }
        
        public DateTime CreateDate { get; set; }
        
        public DateTime? ExpirationDate { get; set; }
        
        public DateTime? ConsumedDate { get; set; }
        
        public string Data { get; set; }
    }
}