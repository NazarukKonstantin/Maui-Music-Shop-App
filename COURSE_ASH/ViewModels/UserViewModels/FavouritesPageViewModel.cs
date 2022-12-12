namespace COURSE_ASH.ViewModels.UserViewModels;

public partial class FavouritesPageViewModel : BaseViewModel
{
    [ObservableProperty]
    ObservableCollection<Product> _favourites;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsNotEmpty))]
    bool isEmpty = true;
    public bool IsNotEmpty => !isEmpty;

    readonly FavouritesService _favouritesService;
    public FavouritesPageViewModel(FavouritesService favouritesService)
    {
        _favouritesService = favouritesService;

        RefreshAsync();
    }
    public async void RefreshAsync()
    {
        Favourites = (await _favouritesService
            .GetFavouritesForUserAsync(App.CurrentLogin))
            .ToObservableCollection();
        IsEmpty = Favourites.Count == 0;
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
        if (IsBusy) return;
        IsBusy = true;
        await _favouritesService
            .DeleteFromFavouritesAsync(App.CurrentLogin, product.ID);
        Favourites = (await _favouritesService
            .GetFavouritesForUserAsync(App.CurrentLogin))
            .ToObservableCollection();
        IsBusy = false;
    }
}
