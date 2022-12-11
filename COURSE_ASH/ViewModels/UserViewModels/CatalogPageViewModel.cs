using CommunityToolkit.Maui.Core.Extensions;

namespace COURSE_ASH.ViewModels.UserViewModels;

public partial class CatalogPageViewModel : BaseViewModel
{
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsNotLoggedIn))]
    bool isLoggedIn=false;
    
    public bool IsNotLoggedIn => !IsLoggedIn;
    
    public List<CatalogItem> Items { get; set; }

    public static List<Product> Products { get; private set; }

    public CatalogPageViewModel(ProductsService productsService)
    {
        Items=CatalogItem.CatalogList;
        Products=productsService.GetProducts();
    }

    [RelayCommand]
    async Task GoToSearchPageAsync(CatalogItem item)
    {
        if (item is null || string.IsNullOrEmpty(item.Category)) return;
        bool IsTabVisible = false;

        ObservableCollection<Product> filteredProducts = (from product in Products
                                                          where product.Category == item.Category
                                                          select product).ToObservableCollection<Product>();
        await Shell.Current.GoToAsync($"{nameof(SearchPage)}", true,
            new Dictionary<string, object>
            {
                ["Products"] = filteredProducts,
                [nameof(IsTabVisible)]=IsTabVisible
            });
    }

    [RelayCommand]
    async Task GoToLoginPage()
    {
        await Shell.Current.GoToAsync($"{nameof(LoginPage)}");
    }
}
