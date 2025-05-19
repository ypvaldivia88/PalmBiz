using Core.Interfaces;
using Microsoft.Extensions.Logging;
using SQLite;
using Infrastructure.Data;
using Infrastructure.Services;

namespace Mobile
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            // Register SQLiteAsyncConnection as a singleton using DatabaseService
            builder.Services.AddSingleton<SQLiteAsyncConnection>(provider =>
            {
                // Ensure DatabaseService.InitializeAsync() is called before this!
                return DatabaseService.GetConnection();
            });

            // Register ProductRepository and ProductService
            builder.Services.AddSingleton<ProductRepository>();
            builder.Services.AddSingleton<IProductService, ProductService>();

            // Register SaleRepository and SaleService
            builder.Services.AddSingleton<SaleRepository>();
            builder.Services.AddSingleton<ISaleService, SaleService>();

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
