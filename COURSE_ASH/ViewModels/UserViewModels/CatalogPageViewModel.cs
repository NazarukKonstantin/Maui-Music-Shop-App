namespace COURSE_ASH.ViewModels.UserViewModels;

[QueryProperty(nameof(ProfileImageLink), nameof(ProfileImageLink))]
[QueryProperty(nameof(ProfileImageRotation), nameof(ProfileImageRotation))]
[QueryProperty(nameof(ProfileImageScale), nameof(ProfileImageScale))]
public partial class CatalogPageViewModel : BaseViewModel, IRefreshable
{
    //Объект сервиса по работе с товарами
    private readonly ProductsService _productService;
    private readonly CatalogService _catalogService;
    //Атрибут, позволяющий удобно реализовать паттерн Наблюдатель
    [ObservableProperty]
    //Поле, хранящее логин вошедшего пользователя
    string _currentLogin;

    [ObservableProperty]
    //Поле, хранящее ссылку на изображение - аватар пользователя
    string _profileImageLink;

    [ObservableProperty]
    private double _profileImageRotation;

    [ObservableProperty]
    private double _profileImageScale;

    [ObservableProperty]
    //Коллекция товаров
    ObservableCollection<Product> _products;

    //Коллекция категорий товаров
    [ObservableProperty]
    ObservableCollection<CatalogItem> _items;

    //Аргумент конструктора - объект сервиса работы с товарами
    public CatalogPageViewModel(ProductsService productsService, CatalogService catalogService)
    {
        _catalogService = catalogService;
        _productService=productsService;
        RefreshAsync();
    }

    //Атрибут, позволяющий удобно реализовать паттерн Команда
    [RelayCommand]
    //Метод отвечает за переход на страницу поиска по товарам выбранной категории
    //Аргумент - объект выбранной категории
    async Task GoToSearchPageAsync(CatalogItem item)
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
    //Метод перехода на страницу профиля пользователя
    async Task GoToProfilePageAsync()
    {
        await Shell.Current.GoToAsync($"{nameof(ProfilePage)}", true,
            new Dictionary<string, object>
            {
                //Передача странице ссылки на изображение - аватар пользователя
                ["ImageLink"] = ProfileImageLink,
                ["IsNotAdmin"] = true,
                ["ImageRotation"]=ProfileImageRotation,
                ["ImageScale"]=ProfileImageScale
            });
    }

    [RelayCommand]
    //Метод отвечает за выход из аккаунта
    private async void SignOut()
    {
        bool choice = await Shell.Current.DisplayAlert("Are you sure?", "Do you want to sign out?", "Yes", "No");
        if (choice)
        {
            CurrentLogin = string.Empty;
            ProfileImageLink = string.Empty;
            ProfileImageRotation = 0;
            ProfileImageScale = 1;
            App.CurrentLogin = string.Empty;
            //Переход на страницу авторизации
            App.Current.MainPage = new AuthorizationShell();
        }
    }

    [RelayCommand]
    //Метод отвечает за обновление страницы
    public async void RefreshAsync()
    {
        try
        {
            IsRefreshing = true;
            IsBusy = true;
            CurrentLogin = App.CurrentLogin;
            //Получение ссылки на изображение из БД
            if (string.IsNullOrEmpty(ProfileImageLink))
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
            Items = (await _catalogService.GetCategoriesAsync()).ToObservableCollection();
            Products = (await _productService.GetProductsAsync()).ToObservableCollection();
        }
        catch (Exception)
        {
            //await Shell.Current.DisplayAlert("ERROR", "Could not load items!", "OK");
            await Toast.Make(GeneralAlerts.NO_CONNECTION, ToastDuration.Short).Show();
        }
        finally
        {
            IsRefreshing = false;
            IsBusy = false;
        }
    }
}
