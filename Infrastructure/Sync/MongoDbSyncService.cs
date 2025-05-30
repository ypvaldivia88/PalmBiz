using System.Text.Json;
using Core.Entities;
using Core.Interfaces;
using MongoDB.Bson;
using MongoDB.Driver;
using SQLite;
using Xamarin.Essentials;

namespace Infrastructure.Sync
{
    public class MongoDbSyncService : ISyncService
    {
        private readonly SQLiteAsyncConnection _localDb;
        private readonly IMongoDatabase _database;
        private readonly string _connectionString;
        
        public MongoDbSyncService(SQLiteAsyncConnection localDb)
        {
            _localDb = localDb;
            
            // Your free tier MongoDB Atlas connection string
            _connectionString = "mongodb+srv://username:password@cluster0.mongodb.net/palmBizDb?retryWrites=true&w=majority";
            
            var settings = MongoClientSettings.FromConnectionString(_connectionString);
            settings.ServerApi = new ServerApi(ServerApiVersion.V1);
            var client = new MongoClient(settings);
            _database = client.GetDatabase("palmBizDb");
        }
        
        public async Task InitializeAsync()
        {
            // Create SyncStatus table if not exists
            await _localDb.CreateTableAsync<SyncStatus>();
            
            // Ensure collections exist in MongoDB
            var collections = await _database.ListCollectionNamesAsync();
            var collectionNames = await collections.ToListAsync();
            
            if (!collectionNames.Contains("products"))
                await _database.CreateCollectionAsync("products");
            
            if (!collectionNames.Contains("sales"))
                await _database.CreateCollectionAsync("sales");
                
            if (!collectionNames.Contains("saleDetails"))
                await _database.CreateCollectionAsync("saleDetails");
                
            if (!collectionNames.Contains("users"))
                await _database.CreateCollectionAsync("users");
                
            if (!collectionNames.Contains("exchangeRates"))
                await _database.CreateCollectionAsync("exchangeRates");
                
            if (!collectionNames.Contains("syncStatus"))
                await _database.CreateCollectionAsync("syncStatus");
        }
        
        public async Task<bool> SyncAllAsync()
        {
            if (!await IsConnectedAsync())
                return false;
                
            try
            {
                // Sync all entity types
                await SyncCollectionAsync<Product>("products");
                await SyncCollectionAsync<Sale>("sales");
                await SyncCollectionAsync<SaleDetail>("saleDetails");
                await SyncCollectionAsync<User>("users");
                await SyncCollectionAsync<ExchangeRate>("exchangeRates");
                
                // Update last sync time
                await SetLastSyncTimeAsync(DateTime.UtcNow);
                return true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Sync error: {ex.Message}");
                return false;
            }
        }
        
        public async Task<bool> SyncEntityAsync<T>(T entity) where T : BaseEntity
        {
            if (!await IsConnectedAsync())
                return false;
                
            try
            {
                var collectionName = typeof(T).Name.ToLower() + "s"; // products, sales, etc.
                var collection = _database.GetCollection<BsonDocument>(collectionName);
                
                // Update LastModified timestamp
                entity.LastModified = DateTime.UtcNow;
                
                // Convert entity to BsonDocument
                var json = JsonSerializer.Serialize(entity);
                var doc = BsonDocument.Parse(json);
                
                // Add or update entity in MongoDB
                var filter = Builders<BsonDocument>.Filter.Eq("SyncId", entity.SyncId);
                var options = new ReplaceOptions { IsUpsert = true };
                await collection.ReplaceOneAsync(filter, doc, options);
                
                // Update local entity status
                entity.IsSynced = true;
                await _localDb.UpdateAsync(entity);
                
                return true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Sync entity error: {ex.Message}");
                return false;
            }
        }
        
async Task SyncCollectionAsync<T>(string collectionName) where T : BaseEntity, new()
        {
            // Get MongoDB collection
            var collection = _database.GetCollection<BsonDocument>(collectionName);

            // 1. Push local changes to MongoDB
            var pendingItems = await _localDb.Table<T>()
                .Where(e => !e.IsSynced || e.IsDeleted)
                .ToListAsync();

            foreach (var item in pendingItems)
            {
                if (item.IsDeleted)
                {
                    // Delete from MongoDB
                    var deleteFilter = Builders<BsonDocument>.Filter.Eq("SyncId", item.SyncId);
                    await collection.DeleteOneAsync(deleteFilter);

                    // If completely synced, delete locally too
                    await _localDb.DeleteAsync(item);
                }
                else
                {
                    // Update LastModified timestamp
                    item.LastModified = DateTime.UtcNow;

                    // Convert to BsonDocument and upsert to MongoDB
                    var json = JsonSerializer.Serialize(item);
                    var doc = BsonDocument.Parse(json);

                    var upsertFilter = Builders<BsonDocument>.Filter.Eq("SyncId", item.SyncId);
                    var options = new ReplaceOptions { IsUpsert = true };
                    await collection.ReplaceOneAsync(upsertFilter, doc, options);

                    // Mark as synced locally
                    item.IsSynced = true;
                    await _localDb.UpdateAsync(item);
                }
            }

            // 2. Pull changes from MongoDB to local
            var lastSync = await GetLastSyncTimeAsync();
            var pullFilter = Builders<BsonDocument>.Filter.Gt("LastModified", lastSync.ToUniversalTime());
            var serverItems = await collection.Find(pullFilter).ToListAsync();

            foreach (var doc in serverItems)
            {
                // Extract SyncId for querying
                var syncId = doc["SyncId"].AsString;

                // Check if item already exists locally
                var localItem = await _localDb.Table<T>()
                    .Where(e => e.SyncId == syncId)
                    .FirstOrDefaultAsync();

                // Convert BsonDocument to entity
                var json = doc.ToJson();
                var item = JsonSerializer.Deserialize<T>(json);

                if (localItem == null)
                {
                    // New item, add to local DB
                    item.Id = 0; // Reset ID to auto-increment
                    item.IsSynced = true;
                    await _localDb.InsertAsync(item);
                }
                else
                {
                    // Update existing item
                    item.Id = localItem.Id; // Preserve local ID
                    item.IsSynced = true;
                    await _localDb.UpdateAsync(item);
                }
            }
        }
        
        public async Task<DateTime> GetLastSyncTimeAsync()
        {
            var status = await _localDb.Table<SyncStatus>()
                .FirstOrDefaultAsync();
                
            return status?.LastSync ?? DateTime.MinValue;
        }
        
        public async Task SetLastSyncTimeAsync(DateTime time)
        {
            var status = await _localDb.Table<SyncStatus>()
                .FirstOrDefaultAsync();
                
            if (status == null)
            {
                status = new SyncStatus { LastSync = time };
                await _localDb.InsertAsync(status);
            }
            else
            {
                status.LastSync = time;
                await _localDb.UpdateAsync(status);
            }
        }
        
        public async Task<bool> IsConnectedAsync()
        {
            return Connectivity.NetworkAccess == Xamarin.Essentials.NetworkAccess.Internet;
        }
    }
    
    public class SyncStatus
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public DateTime LastSync { get; set; }
    }
}