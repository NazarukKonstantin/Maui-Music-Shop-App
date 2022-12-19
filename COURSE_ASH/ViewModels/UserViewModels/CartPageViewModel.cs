namespace COURSE_ASH.ViewModels.UserViewModels;

public partial class CartPageViewModel : BaseViewModel
{
    private readonly CartService _cartService;

    
    [ObservableProperty] //Атрибут, позволяющий реализовать паттерн Наблюдатель
    private ObservableCollection<CartProduct> _products; //Список товаров в корзине

    [ObservableProperty]
    //Итоговая цена покупки
    private double _totalPrice;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsNotEmpty))]
    //Поле, описывающее состояние корзины как пустой
    private bool isEmpty = true;

    //Поле, описывающее состояние корзины как не пустой
    public bool IsNotEmpty => !isEmpty;

    //В конструктор передаётся объект сервиса по работе с корзиной
    public CartPageViewModel(CartService cartService)
    {
        _cartService = cartService;
        RefreshAsync();
    }

    //Асинхронный метод, позволяющий получить корзину пользователя из БД
    async void GetCart()
    {
        Products = (await _cartService.GetCartFromStorageAsync(App.CurrentLogin))?
            .ToObservableCollection();
    }

    [RelayCommand] //Атрибут, позволяющий реализовать паттерн Команда
    //Асинхронный метод, уменьшающий количество товара в корзине при нажатии кнопки
    //Аргументом выступает данный продукт
    async void RemoveProduct(CartProduct product)
    {
        IsBusy = true;
        //Уменьшение количества товара в БД
        CartProduct removingProduct = await _cartService
            .RemoveProductFromStorageCartAsync(App.CurrentLogin,product);

        //Если количество данного товара равно 0, удалить товар из локального списка
        if (removingProduct.UnitQuantity == 0) Products.Remove(product);
        //Иначе присвоить данному товару в списке объект изменённого товара из БД
        else Products[Products.IndexOf(product)] = removingProduct;
        IsEmpty = !Products.Any();
        IsBusy = false;
    }
    [RelayCommand]
    //Метод, полностью удаляющий товар из корзины
    //Аргументом является данный товар
    async void DeleteProduct(CartProduct product)
    {
        IsBusy = true;
        //Удаление товара из БД
        await _cartService
            .DeleteProductFromStorageCartAsync(App.CurrentLogin, product);
        //Удаление товара из локального списка
        Products.Remove(product);
        IsEmpty = !Products.Any();
        IsBusy = false;
    }

    [RelayCommand]
    //Метод, позволяющий добавить в корзину продукт
    //Аргументом выступает данный товар
    async Task AddProduct(CartProduct product)
    {
        IsBusy = true;
        //Добавление количества товара в БД
        CartProduct newProduct = await _cartService
            .AddProductToStorageCartAsync(App.CurrentLogin, product);
        //Присвоение данному товару в списке объекта изменённого товара из БД
        Products[Products.IndexOf(product)] = newProduct;
        IsBusy = false;
    }

    [RelayCommand]
    //Метод, позволяющий перейти на страницу данного товара из корзины
    //Аргументом выступает данный товар
    async Task GoToProduct(CartProduct currentProduct)
    {
        await Shell.Current.GoToAsync($"{nameof(ProductPage)}",
            new Dictionary<string, object>
            {
                //Передача странице товара объекта данного товара
                ["CurrentProduct"] = currentProduct.GetProduct()
            });
    }

    [RelayCommand]
    //Метод отвечает за переход на страницу оформления заказа
    async Task CheckoutAsync()
    {
        if (!Products.Any()) return;
        await Shell.Current.GoToAsync($"{nameof(CheckoutPage)}",
            new Dictionary<string, object>
            {
                //Передача странице оформления заказа объекта данного товара и итоговой цены
                ["Products"] = Products,
                ["TotalPrice"] = TotalPrice,
            });
    }

    [RelayCommand]
    //Метод отвечает за подсчёт итоговой суммы заказа
    void CountTotal()
    {
        TotalPrice = 0;
        if (!Products.Any())
            return;

        foreach (CartProduct product in Products)
        {
            TotalPrice += product.Price * product.UnitQuantity;
        }
    }

    [RelayCommand]
    //Метод отвечает за переход из корзины на страницу поиска товаров 
    async Task GoToSearchPageAsync()
    {
        var loc = Shell.Current.CurrentState;
        Console.WriteLine(loc);
        await Shell.Current.GoToAsync("Search");
    }

    //Метод отвечает за обновление экрана корзины
    public void RefreshAsync()
    {
        PropertyChanged += CollectionChanged;
        GetCart();
    }

    //Метод отвечает за обработку события по изменению списка товаров
    void CollectionChanged(object sender, PropertyChangedEventArgs e)
    {
        //Если изменение списка не затронула объект Products, выйти из метода
        if (e.PropertyName != nameof(Products)) return;
        //Иначе изменить статус поля IsEmpty в зависимости от длины списка Products
        IsEmpty = Products.Count == 0;
    }
}
