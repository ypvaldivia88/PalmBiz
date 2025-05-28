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

        public bool IsRefreshing
        {
            get => _isRefreshing;
            set => SetProperty(ref _isRefreshing, value);
        }

        public Command RefreshCommand { get; }

        public ProductViewModel(IProductService productService)
        {
            Title = "Productos";
            _productService = productService;
            RefreshCommand = new Command(async () => await LoadProductsAsync());
        }

        public async Task LoadProductsAsync()
        {
            if (IsBusy) return;

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
