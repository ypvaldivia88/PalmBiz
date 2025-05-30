using Core.Entities;
using Core.Interfaces;
using System.Collections.ObjectModel;

namespace Mobile.ViewModels
{
    public class ProductViewModel : BaseViewModel
    {
        private readonly IProductService _productService;
        public ObservableCollection<Product> Products { get; } = new();
        private bool _isRefreshing;
        private bool _isSyncing;

        public bool IsRefreshing
        {
            get => _isRefreshing;
            set => SetProperty(ref _isRefreshing, value);
        }

        public bool IsSyncing
        {
            get => _isSyncing;
            set => SetProperty(ref _isSyncing, value);
        }

        public Command RefreshCommand { get; }
        public Command SyncCommand { get; }

        public ProductViewModel(IProductService productService)
        {
            Title = "Productos";
            _productService = productService;
            RefreshCommand = new Command(async () => await LoadProductsAsync());
            SyncCommand = new Command(async () => await SyncDataAsync());
        }

        private async Task SyncDataAsync()
        {
            if (IsBusy || IsSyncing)
                return;

            try
            {
                IsSyncing = true;

                var success = await DatabaseService.SyncAsync();

                if (success)
                {
                    await Shell.Current.DisplayAlert("Éxito", "Sincronización completada correctamente", "OK");
                    await LoadProductsAsync();
                }
                else
                {
                    await Shell.Current.DisplayAlert("Error", "No se pudo sincronizar con el servidor", "OK");
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", $"Error en sincronización: {ex.Message}", "OK");
            }
            finally
            {
                IsSyncing = false;
            }
        }

        public async Task LoadProductsAsync()
        {
            if (IsBusy)
                return;

            try
            {
                IsBusy = true;
                var products = await _productService.GetAllAsync();
                Products.Clear();
                foreach (var product in products)
                {
                    Products.Add(product);
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", "No se pudieron cargar los productos", "OK");
            }
            finally
            {
                IsBusy = false;
                IsRefreshing = false;
            }
        }
    }
}
