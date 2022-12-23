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
    [NotifyPropertyChangedFor(nameof(IsNotInCart))]
    private bool _isInCart = false;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsNotFavourite))]
    private bool _isFavourite = false;

    [ObservableProperty]
    private bool _isReviewNotEmpty = false;

    public bool IsNotFavourite => !IsFavourite;
    public bool IsNotInCart => !IsInCart;

    public ProductPageViewModel(CartService cartService, FavouritesService favouritesService, ProductsService productsService)
    {
        _cartService = cartService;
        _favouritesService = favouritesService;
        _productsService=productsService;
        GetReviewsAsync();
        PropertyChanged += CheckReviewEmpty;
        PropertyChanged += ProductPropertyChanged;
    }

    private void CheckReviewEmpty(object sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName != nameof(Review)) return;
        IsReviewNotEmpty = !string.IsNullOrEmpty(Review);
    }

    [RelayCommand]
    private async void GetReviewsAsync()
    {
        try
        {
            IsBusy = true;
            IsRefreshing = true;
            ReviewList = (await _productsService.GetReviewsForAsync(PickedProduct)).ToObservableCollection();
        }
        catch
        {
            //await Shell.Current.DisplayAlert("ERROR", "Could not load reviews!", "OK");
            await Toast.Make(GeneralAlerts.NO_CONNECTION, ToastDuration.Short).Show();
        }
        finally
        {
            IsBusy = false;
            IsRefreshing = false;
        }
    }
    public async void ProductPropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName != nameof(PickedProduct)) return;
        IsBusy = true;
        try
        {
            IsInCart = await _cartService.IsProductInStorageCart(PickedProduct, App.CurrentLogin);
            IsFavourite = await _favouritesService.IsProductFavouriteAsync(App.CurrentLogin, PickedProduct);
        }
        catch
        {
            await Toast.Make(GeneralAlerts.NO_CONNECTION, ToastDuration.Short).Show();
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    private async Task SaveReviewAsync(Review review)
    {
        try
        {
            IsBusy = true;
            await _productsService.AddReviewForAsync(PickedProduct,review);
        }
        catch
        {
            //await Shell.Current.DisplayAlert("ERROR", "Could not save review!", "OK");
            await Toast.Make(GeneralAlerts.NO_CONNECTION, ToastDuration.Short).Show();
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    async Task AddToCartAsync(Product product)
    {
        try
        {
            IsBusy = true;
            await _cartService
                .AddProductToStorageCartAsync(App.CurrentLogin,
                    new CartProduct(product) { UnitQuantity = 0 });
            await Toast.Make(GeneralAlerts.ADDED_TO_CART, ToastDuration.Short).Show();
            IsInCart = true;
        }
        catch
        {
            //await Shell.Current.DisplayAlert("ERROR", "Could not add to cart!", "OK");
            await Toast.Make(GeneralAlerts.NO_CONNECTION, ToastDuration.Short).Show();
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    async Task RemoveFromCartAsync(Product product)
    {
        try
        {
            IsBusy = true;
            await _cartService
                .DeleteProductFromStorageCartAsync(App.CurrentLogin, product);
            await Toast.Make(GeneralAlerts.REMOVED_FROM_CART,
                    ToastDuration.Short).Show();
            IsInCart = false;
        }
        catch (Exception)
        {
            //await Shell.Current.DisplayAlert("ERROR", "Could not remove from cart!", "OK");
            await Toast.Make(GeneralAlerts.NO_CONNECTION, ToastDuration.Short).Show();
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    async Task ChangeFavouriteStatusAsync()
    {
        try
        {
            IsBusy = true;
            if (IsFavourite)
                await _favouritesService.DeleteFromFavouritesAsync(App.CurrentLogin, PickedProduct);
            else await _favouritesService.SetFavouriteAsync(App.CurrentLogin, PickedProduct);
            IsFavourite = !IsFavourite;
        }
        catch
        {
            //await Shell.Current.DisplayAlert("ERROR", "Could not change favourite status!", "OK");
            await Toast.Make(GeneralAlerts.NO_CONNECTION, ToastDuration.Short).Show();
        }
        finally
        {
            IsBusy = false;
        }

    }

    [RelayCommand]
    async Task GoBackAsync()
    {
        await Shell.Current.GoToAsync("..");
    }
}
