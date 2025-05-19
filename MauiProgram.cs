using Infrastructure.Data;
using Infrastructure.Services;
using Core.Interfaces;
using SQLite;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();

        // ... other builder configuration

        // Register SQLiteAsyncConnection as a singleton using DatabaseService
        builder.Services.AddSingleton<SQLiteAsyncConnection>(provider =>
        {
            // Ensure DatabaseService.InitializeAsync() is called before this!
            return DatabaseService.GetConnection();
        });

        // Register ProductRepository and ProductService
        builder.Services.AddSingleton<ProductRepository>();
        builder.Services.AddSingleton<IProductService, ProductService>();

        return builder.Build();
    }
}