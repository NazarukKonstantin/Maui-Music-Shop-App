using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;

namespace COURSE_ASH.ViewModels;

[QueryProperty(nameof(PickedProduct),nameof(PickedProduct))]
public partial class ProductPageViewModel : BaseViewModel
{
    private readonly CartService _cartService;
    private readonly FavouritesService _favouritesService;
    private readonly ProductsService _productsService;

    [ObservableProperty]
    private Product _pickedProduct;

    [ObservableProperty]
    private ObservableCollection<Review> _reviewList;

    [ObservableProperty]
    private string _review;

    [ObservableProperty]
    private int _rating;

    [ObservableProperty]
    private string _currentLogin;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsNotInCart))]
    private bool _isInCart = false;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsNotFavourite))]
    private bool _isFavourite = false;

    public bool IsNotFavourite => !IsFavourite;
    public bool IsNotInCart => !IsInCart;

    public ProductPageViewModel(CartService cartService, FavouritesService favouritesService, ProductsService productsService)
    {
        _cartService = cartService;
        _favouritesService = favouritesService;
        _productsService=productsService;
        GetReviewsAsync();
        PropertyChanged += ProductPropertyChanged;
    }

    private async void GetReviewsAsync()
    {
        ReviewList=(await _productsService.GetReviewsForAsync(PickedProduct)).ToObservableCollection();
    }
    public async void ProductPropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName != nameof(PickedProduct)) return;
        IsInCart = await _cartService.IsProductInStorageCart(PickedProduct, App.CurrentLogin);
        IsFavourite = await _favouritesService.IsProductFavouriteAsync(App.CurrentLogin, PickedProduct);
    }

    [RelayCommand]
    private async Task SaveReview()
    {

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
