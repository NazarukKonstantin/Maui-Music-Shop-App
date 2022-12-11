using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;

namespace COURSE_ASH.ViewModels.UserViewModels;

[QueryProperty(nameof(PickedProduct),nameof(PickedProduct))]
public partial class ProductPageViewModel : BaseViewModel
{
    private readonly CartService _cartService;
    private readonly FavouritesService _favouritesService;

    [ObservableProperty]
    private Product _pickedProduct;

    [ObservableProperty]
    private int _countInCart;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(NotInCart))]
    private bool _inCart = false;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsNotFavouriteForUser))]
    private bool _isFavouriteForUser = false;

    public bool IsNotFavouriteForUser => !_isFavouriteForUser;
    public bool NotInCart => !_inCart;

    public ProductPageViewModel(CartService cartService, FavouritesService favouritesService)
    {
        _cartService = cartService;
        _favouritesService = favouritesService;

        PropertyChanged += ProductPropertyChanged;
    }
    public async void ProductPropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName != nameof(PickedProduct)) return;
        InCart = await _cartService.IsProductInStorageCart(PickedProduct, App.CurrentLogin);
        IsFavouriteForUser = await _favouritesService.IsProductFavouriteForUserAsync(App.CurrentLogin, PickedProduct.ID);
    }

    [RelayCommand]
    async Task AddToCart(Product product)
    {
        IsBusy = true;
        await _cartService
            .AddProductToStorageCartAsync(App.CurrentLogin,
                new CartProduct(product) { UnitQuantity = CountInCart });


        string text = "Added to cart";
        await Toast.Make(text, ToastDuration.Short).Show();

        InCart = true;
        IsBusy = false;
    }

    [RelayCommand]
    async Task RemoveFromCart(Product product)
    {
        IsBusy = true;
        await _cartService.DeleteProductFromStorageCartAsync(App.CurrentLogin, product);


        string text = "Removed from cart";
        await Toast.Make(text, ToastDuration.Short).Show();

        InCart = false;
        IsBusy = false;
    }

    [RelayCommand]
    async Task AddToFavourites()
    {
        IsBusy = true;
        if (IsFavouriteForUser)
            await _favouritesService.DeleteFromFavouritesAsync(App.CurrentLogin, PickedProduct.ID);
        else await _favouritesService.SetFavouriteAsync(App.CurrentLogin, PickedProduct.ID);
        IsFavouriteForUser = !IsFavouriteForUser;
        IsBusy = false;
    }
}
