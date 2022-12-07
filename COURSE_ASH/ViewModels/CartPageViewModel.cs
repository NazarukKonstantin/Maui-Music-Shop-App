namespace COURSE_ASH.ViewModels;

public partial class CartPageViewModel : BaseViewModel
{
    readonly CartService _cartService;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsNotSignedIn))]
    bool isSignedIn;
    public bool IsNotSignedIn => !isSignedIn;

    [ObservableProperty]
    ObservableCollection<Product> products;

    [ObservableProperty]
    double _totalPrice;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsNotEmpty))]
    bool isEmpty = true;
    public bool IsNotEmpty => !isEmpty;

    public CartPageViewModel(CartService cartService)
    {
        _cartService = cartService;
        _cartService.CartChanged += UpdateCart;
        PropertyChanged += CollectionChanged;

        IsSignedIn = App.CurrentAccount.IsLoggedIn;
        App.CurrentAccount.PropertyChanged += AccountStateUpdated;

        Products = new(_cartService.GetCartList());
    }
    void UpdateCart()
    {
        Products = _cartService.GetCartList().ToObservableCollection<Product>();
    }

    [RelayCommand]
    async void RemoveProduct(Product product)
    {
        Products = new(await _cartService.RemoveProduct(product));
    }

    [RelayCommand]
    async Task AddProduct(Product product)
    {
        Products = new(await _cartService.AddProduct(product));
    }

    [RelayCommand]
    async Task GoToProduct(Product pickedProduct)
    {
        await Shell.Current.GoToAsync($"{nameof(ProductPage)}",
            new Dictionary<string, object>
            {
                ["PickedProduct"] = pickedProduct
            });
    }

    [RelayCommand]
    async Task GoToSearchPageAsync()
    {
        var loc = Shell.Current.CurrentState;
        Console.WriteLine(loc);
        await Shell.Current.GoToAsync("Search");
    }

    [RelayCommand]
    async Task CheckoutAsync()
    {
        //TODO
    }

    //[RelayCommand]
    //async Task Checkout()
    //{
    //    if (!Products.Any()) return;
    //    double totalPrice = 0;
    //    foreach (Product product in Products)
    //    {
    //        totalPrice += product.Price * product.Quantity;
    //    }
    //    await Shell.Current.GoToAsync($"{nameof(CheckoutView)}",
    //        new Dictionary<string, object>
    //        {
    //            ["Products"] = Products,
    //            ["UserName"] = App.UserAccount.UserName,
    //            ["TotalPrice"] = totalPrice
    //        });
    //}

    [RelayCommand]
    void CountTotal()
    {
        TotalPrice = 0;
        if (!Products.Any())
            return;

        foreach (Product product in Products)
        {
            TotalPrice += product.Price * product.UnitQuantity;
        }
    }

    [RelayCommand]
    async void SignIn()
    {
        IsBusy = true;
        await Shell.Current.GoToAsync($"{nameof(LoginPage)}");
        IsBusy = false;
    }

    void AccountStateUpdated(object sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName != nameof(AccountData.IsLoggedIn)) return;
        this.IsSignedIn = !App.CurrentAccount.IsLoggedIn;
        this.IsSignedIn = App.CurrentAccount.IsLoggedIn;
    }
    void CollectionChanged(object sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName != nameof(Products)) return;
        if (Products.Count == 0) IsEmpty = true;
        else IsEmpty = false;
    }
    public void Refresh()
    {
        _cartService.CartChanged += UpdateCart;
        PropertyChanged += CollectionChanged;

        IsSignedIn = App.CurrentAccount.IsLoggedIn;
        App.CurrentAccount.PropertyChanged += AccountStateUpdated;

        Products = new(_cartService.GetCartList());
    }
}
