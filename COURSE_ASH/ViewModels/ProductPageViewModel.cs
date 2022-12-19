using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;

namespace COURSE_ASH.ViewModels;

[QueryProperty(nameof(PickedProduct),nameof(PickedProduct))]
public partial class ProductPageViewModel : BaseViewModel
{
    private readonly CartService _cartService;
    private readonly FavouritesService _favouritesService;

    [ObservableProperty]
    private Product _pickedProduct;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsNotInCart))]
    private bool _isInCart = false;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsNotFavourite))]
    private bool _isFavourite = false;

    public bool IsNotFavourite => !_isFavourite;
    public bool IsNotInCart => !_isInCart;

    public ProductPageViewModel(CartService cartService, FavouritesService favouritesService)
    {
        _cartService = cartService;
        _favouritesService = favouritesService;
        PropertyChanged += ProductPropertyChanged;
    }

    public async void ProductPropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName != nameof(PickedProduct)) return;
        IsInCart = await _cartService.IsProductInStorageCart(PickedProduct, App.CurrentLogin);
        IsFavourite = await _favouritesService.IsProductFavouriteAsync(App.CurrentLogin, PickedProduct);
    }

    [RelayCommand]
    async Task AddToCartAsync(Product product)
    {
        IsBusy = true;
        await _cartService
            .AddProductToStorageCartAsync(App.CurrentLogin,
                new CartProduct(product) { UnitQuantity = 0});

        await Toast.Make(GeneralAlerts.ADDED_TO_CART, ToastDuration.Short).Show();
        IsInCart = true;
        IsBusy = false;
    }

    [RelayCommand]
    async Task RemoveFromCartAsync(Product product)
    {
        IsBusy = true;
        await _cartService
            .DeleteProductFromStorageCartAsync(App.CurrentLogin, product);
        await Toast.Make(GeneralAlerts.REMOVED_FROM_CART,
                ToastDuration.Short).Show();
        IsInCart = false;
        IsBusy = false;
    }

    [RelayCommand]
    async Task ChangeFavouriteStatusAsync()
    {
        IsBusy = true;
        if (IsFavourite)
            await _favouritesService.DeleteFromFavouritesAsync(App.CurrentLogin, PickedProduct);
        else await _favouritesService.SetFavouriteAsync(App.CurrentLogin, PickedProduct);
        IsFavourite = !IsFavourite;
        IsBusy = false;
    }

    [RelayCommand]
    async Task GoBackAsync()
    {
        await Shell.Current.GoToAsync("..");
    }
}
