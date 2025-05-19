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

            // Inicializa la base de datos SINCRÓNICAMENTE antes de registrar el servicio
            DatabaseService.InitializeAsync().GetAwaiter().GetResult();

            // Ahora es seguro registrar la conexión
            builder.Services.AddSingleton<SQLiteAsyncConnection>(provider =>
            {
                return DatabaseService.GetConnection();
            });

            builder.Services.AddSingleton<ProductRepository>();
            builder.Services.AddSingleton<IProductService, ProductService>();
            builder.Services.AddSingleton<SaleRepository>();
            builder.Services.AddSingleton<ISaleService, SaleService>();
            builder.Services.AddSingleton<ExchangeRateRepository>();
            builder.Services.AddSingleton<IExchangeRateService, ExchangeRateService>();

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
