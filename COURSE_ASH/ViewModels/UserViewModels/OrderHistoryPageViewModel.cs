namespace COURSE_ASH.ViewModels.UserViewModels;

[QueryProperty(nameof(CurrentLogin),nameof(CurrentLogin))]
public partial class OrderHistoryPageViewModel : BaseViewModel
{
    private readonly OrderService _orderService;

    [ObservableProperty]
    private ObservableCollection<Order> _orderHistory;

    [ObservableProperty]
    private string _currentLogin;

    [ObservableProperty]
    private int _iD;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsNotEmpty))]
    private bool isEmpty = true;
    public bool IsNotEmpty => !isEmpty;

    public OrderHistoryPageViewModel(OrderService orderService)
    {
        _orderService = orderService;
        PropertyChanged += UserChanged;
        PropertyChanged += OrdersChanged;
        RefreshAsync();
    }
    public async void RefreshAsync()
    {
        if (string.IsNullOrEmpty(CurrentLogin)) return;
        OrderHistory = new ObservableCollection<Order>
            (await _orderService.GetOrdersAsync(CurrentLogin));
    }

    void UserChanged(object sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName != nameof(CurrentLogin)) return;
        RefreshAsync();
    }

    void OrdersChanged(object sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName != nameof(OrderHistory)) return;
        IsEmpty=OrderHistory.Count==0;
    }

    [RelayCommand]
    async Task GoToOrderAsync(Order order)
    {
        await Shell.Current.GoToAsync($"{nameof(OrderPage)}",
            new Dictionary<string, object>
            {
                ["CurrentOrder"] = order
            });
    }
}
