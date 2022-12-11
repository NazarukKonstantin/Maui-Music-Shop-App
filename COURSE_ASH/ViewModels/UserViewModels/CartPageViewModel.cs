namespace COURSE_ASH.ViewModels.UserViewModels;

public partial class CartPageViewModel : BaseViewModel
{
    private readonly CartService _cartService;

    [ObservableProperty]
    private ObservableCollection<CartProduct> _products;

    [ObservableProperty]
    private double _totalPrice;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsNotEmpty))]
    private bool isEmpty = true;
    
    public bool IsNotEmpty => !isEmpty;

    public CartPageViewModel(CartService cartService)
    {
        _cartService = cartService;
        RefreshAsync();
    }

    async void GetCart()
    {
        Products = (await _cartService.GetCartFromStorageAsync(App.CurrentLogin))?
            .ToObservableCollection();
    }

    [RelayCommand]
    async void RemoveProduct(CartProduct product)
    {
        IsBusy = true;
        CartProduct removingProduct = await _cartService
            .RemoveProductFromStorageCartAsync(App.CurrentLogin,product);

        if (removingProduct.UnitQuantity == 0) Products.Remove(product);

        else Products[Products.IndexOf(product)] = removingProduct;
        IsEmpty = !Products.Any();
        IsBusy = false;
    }
    [RelayCommand]
    async void DeleteProduct(CartProduct product)
    {
        IsBusy = true;
        await _cartService
            .DeleteProductFromStorageCartAsync(App.CurrentLogin, product);
        Products.Remove(product);
        IsEmpty = !Products.Any();
        IsBusy = false;
    }

    [RelayCommand]
    async Task AddProduct(CartProduct product)
    {
        IsBusy = true;
        CartProduct newProduct = await _cartService
            .AddProductToStorageCartAsync(App.CurrentLogin, product);
        Products[Products.IndexOf(product)] = newProduct;
        IsBusy = false;
    }

    [RelayCommand]
    async Task GoToProduct(CartProduct currentProduct)
    {
        await Shell.Current.GoToAsync($"{nameof(ProductPage)}",
            new Dictionary<string, object>
            {
                ["CurrentProduct"] = currentProduct.GetProduct()
            });
    }

    [RelayCommand]
    async Task CheckoutAsync()
    {
        if (!Products.Any()) return;
        await Shell.Current.GoToAsync($"{nameof(CheckoutPage)}",
            new Dictionary<string, object>
            {
                ["Products"] = Products,
                ["TotalPrice"] = TotalPrice,
            });
    }

    [RelayCommand]
    void CountTotal()
    {
        TotalPrice = 0;
        if (!Products.Any())
            return;

        foreach (CartProduct product in Products)
        {
            TotalPrice += product.Price * product.UnitQuantity;
        }
    }

    [RelayCommand]
    async Task GoToSearchPageAsync()
    {
        var loc = Shell.Current.CurrentState;
        Console.WriteLine(loc);
        await Shell.Current.GoToAsync("Search");
    }

    public void RefreshAsync()
    {
        PropertyChanged += CollectionChanged;
        GetCart();
    }

    void CollectionChanged(object sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName != nameof(Products)) return;

        IsEmpty = Products.Count == 0;
    }
}
