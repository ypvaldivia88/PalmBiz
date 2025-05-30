using System;

namespace Core.Entities
{
    public abstract class BaseEntity
    {
        public int Id { get; set; }
        
        // MongoDB sync fields
        public string SyncId { get; set; } = Guid.NewGuid().ToString();
        public DateTime LastModified { get; set; } = DateTime.UtcNow;
        public bool IsSynced { get; set; }
        public bool IsDeleted { get; set; }
    }
}