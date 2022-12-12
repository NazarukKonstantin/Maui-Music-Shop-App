using CommunityToolkit.Maui.Core.Extensions;
using COURSE_ASH.Services;

namespace COURSE_ASH.ViewModels.UserViewModels;


public partial class CatalogPageViewModel : BaseViewModel
{
    private readonly ProductsService _service;

    [ObservableProperty]
    private string _currentLogin;

    [ObservableProperty]
    private string _imageLink;

    [ObservableProperty]
    private ObservableCollection<Product> _products;

    public List<CatalogItem> Items { get; set; }

    public CatalogPageViewModel(ProductsService productsService)
    {
        _service=productsService;
        RefreshAsync();
    }

    [RelayCommand]
    async Task GoToSearchPageAsync(CatalogItem item)
    {
        if (item is null || string.IsNullOrEmpty(item.Category)) return;
        bool IsTabVisible = false;

        ObservableCollection<Product> filteredProducts = (from product in Products
                                                          where product.Category == item.Category
                                                          select product).ToObservableCollection();
        await Shell.Current.GoToAsync($"{nameof(SearchPage)}", true,
            new Dictionary<string, object>
            {
                ["Products"] = filteredProducts,
                [nameof(IsTabVisible)]=IsTabVisible
            });
    }

    [RelayCommand]
    async Task GoToProfilePage()
    {
        await Shell.Current.GoToAsync($"{nameof(ProfilePage)}", true,
            new Dictionary<string, object>
            {
                ["ImageLink"]=ImageLink,
            });
    }

    [RelayCommand]
    void SignOut()
    {
        CurrentLogin = string.Empty;
        ImageLink = string.Empty;
        App.CurrentLogin = string.Empty;
        App.Current.MainPage = new AuthorizationShell();
    }

    public async void RefreshAsync()
    {
        CurrentLogin=App.CurrentLogin;
        ImageLink = (await DataStorageService<AccountData>
            .GetItemAsync(nameof(AccountData.CurrentLogin),CurrentLogin))
            .ImageLink ?? string.Empty;

        Items=CatalogItem.CatalogList;
        Products = (await _service.GetProductsAsync()).ToObservableCollection();
    }
}
