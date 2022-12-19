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
        Favourites = (await _favouritesService
            .GetFavouriteProductsAsync(App.CurrentLogin))?
            .ToObservableCollection();
        IsEmpty = Favourites is null || Favourites.Count==0;
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
            .DeleteFromFavouritesAsync(App.CurrentLogin, product);
        Favourites = (await _favouritesService
            .GetFavouriteProductsAsync(App.CurrentLogin))?
            .ToObservableCollection();
        IsEmpty = Favourites is null || Favourites.Count==0;
        IsBusy = false;
    }
}
