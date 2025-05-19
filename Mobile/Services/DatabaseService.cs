using SQLite;
using Core.Entities;

public class DatabaseService
{
    private static SQLiteAsyncConnection _database;

    public static async Task InitializeAsync()
    {
        if (_database != null)
            return;

        var dbPath = Path.Combine(FileSystem.AppDataDirectory, "PalmBiz.db");

        if (!File.Exists(dbPath))
        {
            using var stream = await FileSystem.OpenAppPackageFileAsync("PalmBiz.db");
            using var fileStream = File.Create(dbPath);
            await stream.CopyToAsync(fileStream);
        }

        _database = new SQLiteAsyncConnection(dbPath);
        await _database.CreateTableAsync<Product>();
        await _database.CreateTableAsync<Sale>();
        await _database.CreateTableAsync<SaleDetail>();
        await _database.CreateTableAsync<User>();
        await _database.CreateTableAsync<ExchangeRate>();
    }

    public static SQLiteAsyncConnection GetConnection()
    {
        if (_database == null)
            throw new InvalidOperationException("Database not initialized. Call InitializeAsync() first.");

        return _database;
    }
}
