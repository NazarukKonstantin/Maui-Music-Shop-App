namespace COURSE_ASH.ViewModels.AdminViewModel;

public partial class AdminSearchPageViewModel : BaseViewModel
{
    readonly ProductsService _service;

    [ObservableProperty]
    private ObservableCollection<Product> _products;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsNotEmpty))]
    private bool isEmpty = true;

    public bool IsNotEmpty => !isEmpty;

    public AdminSearchPageViewModel(ProductsService productsService)
    {
        _service = productsService;
        _service.ProductChanged += ProductChanged;
        GetProducts();
    }

    [RelayCommand]
    async Task AddProduct()
    {
        IsBusy = true;
        await Shell.Current.GoToAsync($"{nameof(AddProductPage)}");
        IsBusy = false;
    }

    async void GetProducts()
    {
        Products = (await _service.GetProductsAsync())
            .ToObservableCollection();
        if (Products.Count != 0) IsEmpty = false;
    }

    [RelayCommand]
    async Task EditProduct(Product product)
    {
        IsBusy = true;
        await Shell.Current.GoToAsync($"{nameof(EditProductPage)}",
            new Dictionary<string, object>
            {
                ["Product"] = product
            });
        IsBusy = false;
    }

    [RelayCommand]
    async Task DeleteProduct(Product product)
    {
        IsBusy = true;
        bool deletingConfirmed = await Shell.Current.DisplayAlert("WARNING!",
            $"{product.ProductType} {product.Model} will be deleted",
            "Confirm",
            "Cancel");
        if (deletingConfirmed) 
            await _service.DeleteProductAsync(product);
        IsBusy = false;
    }

    void ProductChanged(object sender, ProductEventArgs e)
    {
        switch (e.State)
        {
            case ProductEventArgs.ProductWas.REMOVED:
                Products.Remove(e.Product);
                if (Products.Count == 0) IsEmpty = true;
                break;
            case ProductEventArgs.ProductWas.CHANGED:
                Products[Products.IndexOf(e.Product)] = e.Product;
                break;
            case ProductEventArgs.ProductWas.ADDED:
                Products.Add(e.Product);
                if (IsEmpty) IsEmpty = false;
                break;
        }
    }
}
