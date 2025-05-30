using SQLite;
using Core.Entities;
using Core.Interfaces;

public class DatabaseService
{
    private static SQLiteAsyncConnection? _database;
    private static ISyncService? _syncService;

    public static async Task InitializeAsync()
    {
        if (_database != null)
            return;

        try
        {
            var dbPath = Path.Combine(FileSystem.AppDataDirectory, "PalmBiz.db");

            if (!File.Exists(dbPath))
            {
                using var stream = await FileSystem.OpenAppPackageFileAsync("PalmBiz.db");
                using var fileStream = File.Create(dbPath);
                await stream.CopyToAsync(fileStream);
            }

            _database = new SQLiteAsyncConnection(dbPath);
            
            // Create tables
            await _database.CreateTableAsync<Product>();
            await _database.CreateTableAsync<Sale>();
            await _database.CreateTableAsync<SaleDetail>();
            await _database.CreateTableAsync<User>();
            await _database.CreateTableAsync<ExchangeRate>();
            await _database.CreateTableAsync<SyncStatus>();
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Database initialization error: {ex}");
            throw;
        }
    }

    public static SQLiteAsyncConnection GetConnection()
    {
        if (_database == null)
            throw new InvalidOperationException("Database not initialized. Call InitializeAsync() first.");

        return _database;
    }
    
    public static void SetSyncService(ISyncService syncService)
    {
        _syncService = syncService;
    }
    
    public static async Task<bool> SyncAsync()
    {
        if (_syncService == null)
            throw new InvalidOperationException("Sync service not initialized.");
            
        return await _syncService.SyncAllAsync();
    }
    
    public static async Task<bool> SyncEntityAsync<T>(T entity) where T : BaseEntity
    {
        if (_syncService == null)
            throw new InvalidOperationException("Sync service not initialized.");
            
        return await _syncService.SyncEntityAsync(entity);
    }
}
