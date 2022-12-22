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
    [NotifyPropertyChangedFor(nameof(IsNotEmpty))]
    private bool isEmpty = true;
    public bool IsNotEmpty => !isEmpty;

    public OrderHistoryPageViewModel(OrderService orderService)
    {
        _orderService = orderService;
        PropertyChanged += OrdersChanged;
        RefreshAsync();
    }
    public async void RefreshAsync()
    {
        try
        {
            IsRefreshing = true;
            IsBusy = true;
            if (string.IsNullOrEmpty(CurrentLogin)) return;
            OrderHistory = new ObservableCollection<Order>
                (await _orderService.GetOrdersAsync(CurrentLogin));
        }
        catch (Exception)
        {
            await Shell.Current.DisplayAlert("ERROR", "Could not load orders!", "OK");
        }
        finally
        {
            IsRefreshing = false;
            IsBusy = false;
        }
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
