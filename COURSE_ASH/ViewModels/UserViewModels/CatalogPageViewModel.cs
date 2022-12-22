namespace COURSE_ASH.ViewModels.UserViewModels;

public partial class CatalogPageViewModel : BaseViewModel
{
    //Объект сервиса по работе с товарами
    private readonly ProductsService _service;

    //Атрибут, позволяющий удобно реализовать паттерн Наблюдатель
    [ObservableProperty]
    //Поле, хранящее логин вошедшего пользователя
    string _currentLogin;

    [ObservableProperty]
    //Поле, хранящее ссылку на изображение - аватар пользователя
    string _imageLink;

    [ObservableProperty]
    //Коллекция товаров
    ObservableCollection<Product> _products;

    //Коллекция категорий товаров
    [ObservableProperty]
    ObservableCollection<CatalogItem> _items;

    //Аргумент конструктора - объект сервиса работы с товарами
    public CatalogPageViewModel(ProductsService productsService)
    {
        _service=productsService;
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
                ["ImageLink"]=ImageLink,
            });
    }

    [RelayCommand]
    //Метод отвечает за выход из аккаунта
    void SignOut()
    {
        CurrentLogin = string.Empty;
        ImageLink = string.Empty;
        App.CurrentLogin = string.Empty;
        //Переход на страницу авторизации
        App.Current.MainPage = new AuthorizationShell();
    }

    //Метод отвечает за обновление страницы
    public async void RefreshAsync()
    {
        try
        {
            IsRefreshing = true;
            IsBusy = true;
            CurrentLogin = App.CurrentLogin;
            //Получение ссылки на изображение из БД
            ImageLink = (await DataStorageService<AccountData>
                        .GetItemByAsync(nameof(AccountData.CurrentLogin), CurrentLogin))
                        .ImageLink ?? string.Empty;
            Items = CatalogItem.CatalogList.ToObservableCollection();
            Products = (await _service.GetProductsAsync()).ToObservableCollection();
        }
        catch (Exception)
        {
            await Shell.Current.DisplayAlert("ERROR", "Could not load items!", "OK");
        }
        finally
        {
            IsRefreshing = false;
            IsBusy = false;
        }
    }
}
