using COURSE_ASH.Models;

namespace COURSE_ASH.ViewModels.AdminViewModels;

public partial class AdminSearchPageViewModel : BaseViewModel
{
    private readonly ProductsService _productsService;
    private readonly CatalogService _catalogService;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsProdListSelected))]
    private bool _isCategListSelected;

    public bool IsProdListSelected { get; set; }

    [ObservableProperty]
    private string _query;

    [ObservableProperty]
    private ObservableCollection<Product> _products;

    [ObservableProperty]
    private List<Product> _prodCacheList;

    [ObservableProperty]
    private ObservableCollection<CatalogItem> _categories;

    [ObservableProperty]
    private List<CatalogItem> _categCacheList;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsProdListNotEmpty))]
    private bool isProdListEmpty = true;
    public bool IsProdListNotEmpty => !isProdListEmpty;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsCategListNotEmpty))]
    private bool isCategListEmpty = true;
    public bool IsCategListNotEmpty => !isCategListEmpty;

    [ObservableProperty]
    private string _currentLogin;

    [ObservableProperty]
    private string _imageLink;

    public AdminSearchPageViewModel(ProductsService productsService, CatalogService catalogService)
    {
        _productsService = productsService;
        _catalogService = catalogService;
        _productsService.ProductChanged += ProductChanged;
        _catalogService.CategoryChanged += CategoryChanged;

        RefreshAsync();
    }

    public async void RefreshAsync()
    {
        IsCategListSelected=true;
        IsProdListSelected=false;
        _categCacheList = (await _catalogService.GetCategoriesAsync()).ToList();
        Categories = _categCacheList.ToObservableCollection();
        _prodCacheList = (await _productsService.GetProductsAsync()).ToList();
        Products = _prodCacheList.ToObservableCollection();
        IsProdListEmpty = Products.Count==0;
        IsCategListEmpty = Categories.Count==0;
        CurrentLogin=App.CurrentLogin;
        ImageLink = (await DataStorageService<AccountData>
                     .GetItemByAsync(nameof(AccountData.CurrentLogin), CurrentLogin))
                     .ImageLink ?? string.Empty;
    }

    [RelayCommand]
    private async Task GoToProfilePageAsync()
    {
        await Shell.Current.GoToAsync($"{nameof(ProfilePage)}", true,
           new Dictionary<string, object>
           {
               ["ImageLink"]=ImageLink,
           });
    }

    [RelayCommand]
    private void SignOut()
    {
        Products = null;
        Categories = null;
        IsProdListEmpty = true;
        IsCategListEmpty = true;
        CurrentLogin = string.Empty;
        ImageLink = string.Empty;
        App.CurrentLogin = string.Empty;
        App.Current.MainPage = new AuthorizationShell();
    }

    private void CheckQuery()
    {
        if (string.IsNullOrWhiteSpace(Query))
        {
            Query = string.Empty;
        }
    }

    [RelayCommand]
    private void SearchProducts()
    {
        CheckQuery();

        var filteredItems = _prodCacheList
                        .Where(value => value.Model
                        .ToLowerInvariant()
                        .Contains(Query
                        .ToLowerInvariant()));

        foreach (var value in _prodCacheList)
        {
            if (!filteredItems.Contains(value))
            {
                Products.Remove(value);
            }
            else if (!Products.Contains(value))
            {
                Products.Add(value);
                Products = Products.OrderBy(p => p.Model).ToObservableCollection();
            }
        }
    }

    [RelayCommand]
    private void SearchCategories()
    {
        CheckQuery();

        var filteredItems = _categCacheList
                        .Where(value => value.Category
                        .ToLowerInvariant()
                        .Contains(Query
                        .ToLowerInvariant()));

        foreach (var value in _categCacheList)
        {
            if (!filteredItems.Contains(value))
            {
                Categories.Remove(value);
            }
            else if (!Categories.Contains(value))
            {
                Categories.Add(value);
                Categories = Categories.OrderBy(c => c.Category).ToObservableCollection();
            }
        }
    }

    [RelayCommand]
    private async Task ChooseCategActionAsync(CatalogItem item)
    {
        if(IsCategListSelected)
        {
            string choice = await Shell.Current.DisplayActionSheet("OPTIONS","Cancel","Delete","Open","Edit");
            switch (choice)
            {
                case "Cancel": return;
                case "Delete": await DeleteCategoryAsync(item); break;
                case "Open": await GoToSearchPageAsync(item); break;
                case "Edit": await ChangeCategoryAsync(item); break;
            }
        }
        
    }

    [RelayCommand]
    private async Task ChooseProdActionAsync(Product prod)
    {
        if (IsProdListSelected)
        {
            string choice = await Shell.Current.DisplayActionSheet("OPTIONS", "Cancel", "Delete", "Open", "Edit");
            switch (choice)
            {
                case "Cancel": return;
                case "Delete": await DeleteProductAsync(prod); break;
                case "Open": await GoToProductAsync(prod); break;
                case "Edit": await ChangeProductAsync(prod); break;
            }
        }
    }

    private async Task GoToProductAsync(Product prod)
    {
        await Shell.Current.GoToAsync($"{nameof(ProductPage)}",
            new Dictionary<string, object>
            {
                ["PickedProduct"] = prod
            });
    }

    private async Task GoToSearchPageAsync(CatalogItem item)
    {
        //Если объект категории null или название категории пусто, выйти из метода
        if (item is null || string.IsNullOrEmpty(item.Category)) return;
        bool IsTabVisible = false;

        //Создание коллекции с продуктами, отфильтрованными в соответствии с выбранной категорией
        ObservableCollection<Product> filteredProducts = (from product in Products
                                                          where product.Category == item.Category
                                                          select product).ToObservableCollection();
        //Переход на страницу поиска
        await Shell.Current.GoToAsync($"{nameof(SearchPage)}", true,
            new Dictionary<string, object>
            {
                //Передача страницу отфильтрованного списка
                ["Products"] = filteredProducts,
                //Передача странице переменной видимости нижних вкладок
                [nameof(IsTabVisible)]=IsTabVisible
            });
    }

    [RelayCommand]
    private async Task AddCategoryAsync()
    {
        IsBusy = true;
        await Shell.Current.GoToAsync($"{nameof(AddCategoryPage)}");
        IsBusy = false;
    }

    [RelayCommand]
    private async Task DeleteCategoryAsync(CatalogItem item)
    {
        IsBusy = true;
        bool choice = await Shell.Current.DisplayAlert("Are you sure?",
            $"{item.Category} with all the products\n" +
            $"of this category will be deleted",
            "Confirm",
            "Cancel");
        if (choice) await _catalogService.RemoveCategoryAsync(item,_productsService);
        IsBusy = false;
    }

    [RelayCommand]
    private async Task ChangeCategoryAsync(CatalogItem item)
    {
        IsBusy = true;
        await Shell.Current.GoToAsync($"{nameof(EditCategoryPage)}",
            new Dictionary<string, object>
            {
                ["Category"] = item
            });
        IsBusy = false;
    }

    [RelayCommand]
    private async Task AddProductAsync()
    {
        IsBusy = true;
        await Shell.Current.GoToAsync($"{nameof(AddProductPage)}");
        IsBusy = false;
    }

    [RelayCommand]
    private async Task DeleteProductAsync(Product product)
    {
        IsBusy = true;
        bool choice = await Shell.Current.DisplayAlert("Are you sure?",
            $"{product.Model} will be deleted",
            "Confirm",
            "Cancel");
        if (choice) await _productsService.DeleteProductAsync(product);
        IsBusy = false;
    }

    [RelayCommand]
    private async Task ChangeProductAsync(Product product)
    {
        IsBusy = true;
        await Shell.Current.GoToAsync($"{nameof(EditProductPage)}",
            new Dictionary<string, object>
            {
                ["CurrentProduct"] = product
            });
        IsBusy = false;
    }

    private void CategoryChanged(object sender, CategoryEventArgs e)
    {
        switch (e.State)
        {
            case CategoryEventArgs.CategoryWas.Added:
                Categories.Add(e.Item);
                if (IsCategListEmpty) IsCategListEmpty = false;
                break;
            case CategoryEventArgs.CategoryWas.Removed:
                Categories.Remove(e.Item);
                IsCategListEmpty = Categories.Count == 0;
                break;
            case CategoryEventArgs.CategoryWas.Changed:
                Categories[Categories.IndexOf(e.Item)] = e.Item;
                break;
        }
    }

    private void ProductChanged(object sender, ProductEventArgs e)
    {
        switch (e.State)
        {
            case ProductEventArgs.ProductWas.Added:
                Products.Add(e.Product);
                if (IsProdListEmpty) IsProdListEmpty = false;
                break;
            case ProductEventArgs.ProductWas.Removed:
                Products.Remove(e.Product);
                DeleteUnusedCategories(e.Product.Category);
                IsProdListEmpty = Products.Count == 0;
                break;
            case ProductEventArgs.ProductWas.Changed:
                Products[Products.IndexOf(e.Product)] = e.Product;
                DeleteUnusedCategories(e.Product.Category);
                break;
        }
    }
    
    private async void DeleteUnusedCategories(string name)
    {
        if (!(from product in Products where product.Category == name select product).Any())
            await _catalogService.RemoveCategoryAsync(name,_productsService);
    }
}