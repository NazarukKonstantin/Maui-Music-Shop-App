namespace COURSE_ASH.ViewModels.AdminViewModels;

public partial class AdminSearchPageViewModel : BaseViewModel
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
    private string _currentLogin;

    [ObservableProperty]
    private string _imageLink;

    [ObservableProperty]
    private double _imageRotation=0;

    [ObservableProperty]
    private double _imageScale = 1;

    public AdminSearchPageViewModel(ProductsService productsService, CatalogService catalogService)
    {
        _productsService = productsService;
        _catalogService = catalogService;
        _productsService.ProductChanged += ProductChanged;
        _catalogService.CategoryChanged += CategoryChanged;
        RefreshAsync();
    }

    [RelayCommand]
    public void RefreshAsync()
    {
        IsRefreshing = true;
        IsBusy = true;
        CurrentLogin = App.CurrentLogin;
        ImageLink = Icons.DefaultProfile;

        //await LoadCategoriesAsync();
        //await LoadProductsAsync();
        //await LoadImageAsync();

        IsProdListEmpty = Products is null || Products.Count == 0;
        IsCategListEmpty = Categories is null ||  Categories.Count == 0;
        IsBusy = false;
        IsRefreshing = false;
    }

    [RelayCommand]
    private void SetList()
    {
        IsCategoryListSelected = !IsCategoryListSelected;
    }

    private async Task LoadCategoriesAsync()
    {
        try
        {
            CategCacheList = (await _catalogService.GetCategoriesAsync()).ToList();
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
            ProdCacheList = (await _productsService.GetProductsAsync()).ToList();
            Products = ProdCacheList.ToObservableCollection();
        }
        catch (Exception)
        {
            //await Shell.Current.DisplayAlert("ERROR", "Could not load products!", "OK");
            await Toast.Make(GeneralAlerts.NO_CONNECTION, ToastDuration.Short).Show();
        }
    }
    private async Task LoadImageAsync()
    {
        try
        {
            ImageLink = (await DataStorageService<AccountData>
                        .GetItemByAsync(nameof(AccountData.CurrentLogin), CurrentLogin))
                        .ImageLink;
            if (string.IsNullOrEmpty(ImageLink))
                ImageLink = Icons.DefaultProfile;
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
               ["ImageLink"] = ImageLink,
               ["IsNotAdmin"] = false,
               ["ImageRotation"]=ImageRotation,
               ["ImageScale"]=ImageScale,
           });
    }

    [RelayCommand]
    private async void SignOut()
    {
        bool choice = await Shell.Current.DisplayAlert("Are you sure?","Do you want to sign out?","Yes","No");
        if (choice)
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
            string choice = await Shell.Current.DisplayActionSheet("OPTIONS", "Cancel", "Delete", "Open", "Edit");
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
            if (choice) await _catalogService.RemoveCategoryAsync(item);
            await Toast.Make("Category deleted",ToastDuration.Short).Show();
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
            if (choice) await _productsService.DeleteProductAsync(product);
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
                IsProdListEmpty = Products.Count == 0;
                break;
            case ProductEventArgs.ProductWas.Changed:
                Products[Products.IndexOf(e.Product)] = e.Product;
                break;
        }
    }
}