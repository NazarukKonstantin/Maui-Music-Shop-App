namespace COURSE_ASH.ViewModels.UserViewModels;

public partial class FavouritesPageViewModel : BaseViewModel
{
    private readonly FavouritesService _favouritesService; 

    [ObservableProperty]
    ObservableCollection<Product> _favourites;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsNotEmpty))]
    private bool isEmpty = true;
    public bool IsNotEmpty => !isEmpty;
    public FavouritesPageViewModel(FavouritesService favouritesService)
    {
        _favouritesService = favouritesService;

        RefreshAsync();
    }
    public async void RefreshAsync()
    {
        try
        {
            IsRefreshing = true;
            IsBusy = true;
            Favourites = (await _favouritesService
                .GetFavouriteProductsAsync(App.CurrentLogin))?
                .ToObservableCollection();
            IsEmpty = Favourites is null || Favourites.Count == 0;
        }
        catch
        {
            //await Shell.Current.DisplayAlert("ERROR", "Could not load favourites!", "OK");
            await Toast.Make(GeneralAlerts.NO_CONNECTION, ToastDuration.Short).Show();
        }
        finally
        {
            IsRefreshing = false;
            IsBusy=false;
        }
    }

    [RelayCommand]
    async Task GoToProduct(Product currentProduct)
    {
        await Shell.Current.GoToAsync($"{nameof(ProductPage)}",
            new Dictionary<string, object>
            {
                ["CurrentProduct"] = currentProduct
            });
    }

    [RelayCommand]
    async Task DeleteFromFavouritesAsync(Product product)
    {
        try
        {
            if (IsBusy) return;
            IsBusy = true;
            await _favouritesService
                .DeleteFromFavouritesAsync(App.CurrentLogin, product);
            Favourites = (await _favouritesService
                .GetFavouriteProductsAsync(App.CurrentLogin))?
                .ToObservableCollection();
        }
        catch
        {
            //await Shell.Current.DisplayAlert("ERROR", "Could not delete from favourites!", "OK");
            await Toast.Make(GeneralAlerts.NO_CONNECTION, ToastDuration.Short).Show();
        }
        finally
        {
            IsEmpty = Favourites is null || Favourites.Count == 0;
            IsBusy = false;
        }
    }
}
