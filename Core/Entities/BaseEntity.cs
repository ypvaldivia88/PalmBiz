namespace Core.Entities
{
    public abstract class BaseEntity
    {
        [SQLite.PrimaryKey, SQLite.AutoIncrement]
        public int Id { get; set; }
        
        // Sync fields
        public bool IsDeleted { get; set; }
        public DateTime LastModified { get; set; }
        public bool IsSynced { get; set; }
        public string SyncId { get; set; } = Guid.NewGuid().ToString();
    }
}