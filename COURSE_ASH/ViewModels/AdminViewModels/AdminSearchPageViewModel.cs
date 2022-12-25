namespace COURSE_ASH.ViewModels.AdminViewModels;

[QueryProperty(nameof(ProfileImageLink), nameof(ProfileImageLink))]
[QueryProperty(nameof(ProfileImageRotation), nameof(ProfileImageRotation))]
[QueryProperty(nameof(ProfileImageScale), nameof(ProfileImageScale))]
public partial class AdminSearchPageViewModel : BaseViewModel, IRefreshable
{
    private readonly ProductsService _productsService;
    private readonly CatalogService _catalogService;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsProductListSelected))]
    bool _isCategoryListSelected;
    public bool IsProductListSelected => !IsCategoryListSelected;

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
    private bool _isProdListEmpty = true;
    public bool IsProdListNotEmpty => !IsProdListEmpty;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsCategListNotEmpty))]
    private bool _isCategListEmpty = true;
    public bool IsCategListNotEmpty => !IsCategListEmpty;

    [ObservableProperty]
    private bool _isEmptyView = false;

    [ObservableProperty]
    private string _currentLogin;

    [ObservableProperty]
    private string _profileImageLink;

    [ObservableProperty]
    private double _profileImageRotation = 0;

    [ObservableProperty]
    private double _profileImageScale = 1;

    public AdminSearchPageViewModel(ProductsService productsService, CatalogService catalogService)
    {
        _productsService = productsService;
        _catalogService = catalogService;
        _productsService.ProductChanged += ProductChanged;
        _catalogService.CategoryChanged += CategoryChanged;
        RefreshAsync();
    }

    [RelayCommand]
    public async void RefreshAsync()
    {
        IsRefreshing = true;
        IsBusy = true;
        CurrentLogin = App.CurrentLogin;

        await LoadCategoriesAsync();
        await LoadProductsAsync();

        if (string.IsNullOrEmpty(ProfileImageLink))
            await LoadProfilePicAsync();

        IsProdListEmpty = Products is null || Products.Count == 0;
        IsCategListEmpty = Categories is null || Categories.Count == 0;
        RecountIsEmptyView();
        IsBusy = false;
        IsRefreshing = false;
        Categories ??= new();
    }

    private void RecountIsEmptyView()
    {
        if (IsCategListEmpty && IsProdListEmpty
            || IsCategListEmpty && IsCategoryListSelected
            || IsProdListEmpty && IsProductListSelected)
            IsEmptyView = true;
        else IsEmptyView = false;
    }

    [RelayCommand]
    private void SetList()
    {
        IsCategoryListSelected = !IsCategoryListSelected;
        RecountIsEmptyView();
    }

    private async Task LoadCategoriesAsync()
    {
        try
        {
            CategCacheList = (await _catalogService.GetCategoriesAsync())?.ToList() ?? new();
        }
        catch (Exception)
        {
            //await Shell.Current.DisplayAlert("ERROR", "Could not load categories!", "OK");
            await Toast.Make(GeneralAlerts.NO_CONNECTION, ToastDuration.Short).Show();
        }
    }
    private async Task LoadProductsAsync()
    {
        try
        {
            Categories = CategCacheList.ToObservableCollection();
            ProdCacheList = (await _productsService.GetProductsAsync())?.ToList() ?? new();
            Products = ProdCacheList.ToObservableCollection();
        }
        catch (Exception)
        {
            //await Shell.Current.DisplayAlert("ERROR", "Could not load products!", "OK");
            await Toast.Make(GeneralAlerts.NO_CONNECTION, ToastDuration.Short).Show();
        }
    }
    private async Task LoadProfilePicAsync()
    {
        try
        {
            AccountData currentAccount = await DataStorageService<AccountData>
            .GetItemByAsync(nameof(AccountData.CurrentLogin), CurrentLogin);
            ProfileImageLink = currentAccount.ImageLink;
            ProfileImageRotation = currentAccount.ImageRotation;
            ProfileImageScale = currentAccount.ImageScale;
            if (string.IsNullOrEmpty(ProfileImageLink))
                ProfileImageLink = App.Current.UserAppTheme == AppTheme.Dark ?
                                    Icons.WaltuhWhite : Icons.WaltuhBlack;
        }
        catch (Exception)
        {
            //await Shell.Current.DisplayAlert("ERROR!", "Could not load images", "OK");
            await Toast.Make(GeneralAlerts.NO_CONNECTION, ToastDuration.Short).Show();
        }
    }

    [RelayCommand]
    private async Task GoToProfilePageAsync()
    {
        await Shell.Current.GoToAsync($"{nameof(ProfilePage)}", true,
           new Dictionary<string, object>
           {
               ["ImageLink"] = ProfileImageLink,
               ["IsNotAdmin"] = false,
               ["ImageRotation"] = ProfileImageRotation,
               ["ImageScale"] = ProfileImageScale,
           });
    }

    [RelayCommand]
    private async void SignOut()
    {
        bool choice = await Shell.Current.DisplayAlert("Are you sure?", "Do you want to sign out?", "Yes", "No");
        if (choice)
        {
            Products = null;
            Categories = null;
            IsProdListEmpty = true;
            IsCategListEmpty = true;
            IsEmptyView = true;
            CurrentLogin = string.Empty;
            ProfileImageLink = string.Empty;
            App.CurrentLogin = string.Empty;
            App.Current.MainPage = new AuthorizationShell();
        }
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
        IsBusy = true;
        CheckQuery();

        var filteredItems = ProdCacheList
                        .Where(value => value.Model
                        .ToLowerInvariant()
                        .Contains(Query
                        .ToLowerInvariant()));

        foreach (var value in ProdCacheList)
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
        IsBusy = false;
    }

    [RelayCommand]
    private void SearchCategories()
    {
        IsBusy = true;
        CheckQuery();

        var filteredItems = CategCacheList
                        .Where(value => value.Category
                        .ToLowerInvariant()
                        .Contains(Query
                        .ToLowerInvariant()));

        foreach (var value in CategCacheList)
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
        IsBusy = false;
    }

    [RelayCommand]
    private async Task ChooseCategActionAsync(CatalogItem item)
    {
        if (IsCategoryListSelected)
        {
            string choice = await Shell.Current.DisplayActionSheet("OPTIONS", "Cancel", null, "Open", "Edit", "Delete");
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
        if (IsProductListSelected)
        {
            string choice = await Shell.Current.DisplayActionSheet("OPTIONS", "Cancel", null, "Open", "Edit", "Delete");
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
                [nameof(IsTabVisible)] = IsTabVisible
            });
    }

    [RelayCommand]
    private async Task AddCategoryAsync()
    {
        await Shell.Current.GoToAsync($"{nameof(AddCategoryPage)}");
    }

    [RelayCommand]
    private async Task DeleteCategoryAsync(CatalogItem item)
    {
        try
        {
            IsBusy = true;
            bool choice = await Shell.Current.DisplayAlert("Are you sure?",
                $"{item.Category} with all the products\n" +
                $"of this category will be deleted",
                "Confirm",
                "Cancel");
            if (choice)
            {
                await _catalogService.RemoveCategoryAsync(item);
                await Toast.Make("Category deleted", ToastDuration.Short).Show();
            }
        }
        catch (Exception)
        {
            //await Shell.Current.DisplayAlert("ERROR", "Could not delete category!", "OK");
            await Toast.Make(GeneralAlerts.NO_CONNECTION, ToastDuration.Short).Show();
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    private async Task ChangeCategoryAsync(CatalogItem item)
    {
        await Shell.Current.GoToAsync($"{nameof(EditCategoryPage)}",
            new Dictionary<string, object>
            {
                ["Category"] = item
            });
    }

    [RelayCommand]
    private async Task AddProductAsync()
    {
        await Shell.Current.GoToAsync($"{nameof(AddProductPage)}");
    }

    [RelayCommand]
    private async Task DeleteProductAsync(Product product)
    {
        try
        {
            IsBusy = true;
            bool choice = await Shell.Current.DisplayAlert("Are you sure?",
                $"{product.Model} will be deleted",
                "Confirm",
                "Cancel");
            if (choice)
                await _productsService.DeleteProductAsync(product);
        }
        catch (Exception)
        {
            //await Shell.Current.DisplayAlert("ERROR", "Could not load products!", "OK");
            await Toast.Make(GeneralAlerts.NO_CONNECTION, ToastDuration.Short).Show();
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    private async Task ChangeProductAsync(Product product)
    {
        await Shell.Current.GoToAsync($"{nameof(EditProductPage)}",
            new Dictionary<string, object>
            {
                ["CurrentProduct"] = product
            });
    }

    private void CategoryChanged(object sender, CategoryEventArgs e)
    {
        switch (e.State)
        {
            case CategoryEventArgs.CategoryWas.Added:
                Categories.Add(e.Item);
                if (IsCategListEmpty)
                    IsCategListEmpty = false;
                break;
            case CategoryEventArgs.CategoryWas.Removed:
                Categories.Remove(e.Item);
                IsCategListEmpty = Categories.Count == 0;
                break;
            case CategoryEventArgs.CategoryWas.Changed:
                Categories[Categories.IndexOf(e.Item)] = e.Item;
                break;
        }
        if (Categories is not null)
            Categories = Categories.OrderBy(c => c.Category).ToObservableCollection();
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
                IsProdListEmpty = Products.Count == 0;
                break;
            case ProductEventArgs.ProductWas.Changed:
                Products[Products.IndexOf(e.Product)] = e.Product;
                break;
        }
        if (Products is not null)
            Products = Products.OrderBy(p => p.Model).ToObservableCollection();
    }
}