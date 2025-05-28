namespace Mobile.Views;

public partial class ProductListView : ContentPage
{
    public ProductListView(ViewModels.ProductViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await ((ViewModels.ProductViewModel)BindingContext).LoadProductsAsync();
    }
}
