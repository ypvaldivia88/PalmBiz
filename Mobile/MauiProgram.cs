using Core.Interfaces;
using Microsoft.Extensions.Logging;
using SQLite;
using Infrastructure.Data;
using Infrastructure.Services;
using Mobile.ViewModels;
using Mobile.Views;

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

            // Initialize database
            DatabaseService.InitializeAsync().GetAwaiter().GetResult();
            builder.Services.AddSingleton<SQLiteAsyncConnection>(provider => DatabaseService.GetConnection());
            
            // Register sync service
            builder.Services.AddSingleton<ISyncService, MongoDbSyncService>();

            // Register repositories
            builder.Services.AddSingleton<ProductRepository>();
            builder.Services.AddSingleton<IProductService, ProductService>();
            builder.Services.AddSingleton<SaleRepository>();
            builder.Services.AddSingleton<ISaleService, SaleService>();            
            builder.Services.AddSingleton<ExchangeRateRepository>();
            builder.Services.AddSingleton<IExchangeRateService, ExchangeRateService>();
            builder.Services.AddSingleton<UserRepository>();
            builder.Services.AddSingleton<IUserService, UserService>();
            
            // Register auth service
            builder.Services.AddSingleton<IAuthService, AuthService>();
            
            // Register Views and ViewModels
            builder.Services.AddSingleton<Views.ProductListView>();
            builder.Services.AddSingleton<ViewModels.ProductViewModel>();
            
            // Register auth related views and viewmodels
            builder.Services.AddTransient<LoginView>();
            builder.Services.AddTransient<LoginViewModel>();
            
            // Register app shell for dependency injection
            builder.Services.AddSingleton<AppShell>();
            
            // Initialize sync service
            var app = builder.Build();
            var syncService = app.Services.GetRequiredService<ISyncService>();
            DatabaseService.SetSyncService(syncService);
            syncService.InitializeAsync().GetAwaiter().GetResult();

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return app;
        }
    }
}
