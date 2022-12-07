namespace COURSE_ASH.ViewModels;

[QueryProperty(nameof(PickedProduct),nameof(PickedProduct))]
public partial class ProductPageViewModel : BaseViewModel
{
    readonly CartService _cartService;

    [ObservableProperty]
    Product _pickedProduct;

    [ObservableProperty]
    int _countInCart;

    public ProductPageViewModel(CartService cartService)
    {
        _cartService = cartService;

        PropertyChanged += ProductPropertyChanged;
    }
    public async void ProductPropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName != nameof(PickedProduct)) return;
        CountInCart = await _cartService.CountProductInCart(PickedProduct.ID);
    }

    [RelayCommand]
    async Task AddToCart(Product product)
    {
        List<Product> newCartList = await _cartService.AddProduct(product);
        CountInCart = (from pr in newCartList where pr.ID == PickedProduct.ID select pr.UnitQuantity).First<int>();
    }

    [RelayCommand]
    async Task GoBackAsync()
    {
        await Shell.Current.GoToAsync("..");
    }
}
