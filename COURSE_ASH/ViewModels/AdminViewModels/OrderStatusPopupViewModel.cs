namespace COURSE_ASH.ViewModels.AdminViewModels;
public partial class OrderStatusPopupViewModel : BaseViewModel
{
    public static readonly List<string> orderStatuses = OrderStatus.Statuses;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(CanBeCancelled))]
    Order _currentOrder;

    [ObservableProperty]
    bool _isStatusChanged = false;

    [ObservableProperty]
    string _status;

    public bool CanBeCancelled => Order.CanBeCancelled(CurrentOrder?.Status ?? null);

    private readonly OrderService _service;
    string _oldStatus;
    public OrderStatusPopupViewModel(OrderService orderService, Order currentOrder)
    {
        _service = orderService;
        CurrentOrder = currentOrder;
        PropertyChanged += OrderChanged;
        PropertyChanged += StatusChanged;
    }

    public void OrderChanged(object sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName != nameof(CurrentOrder)) return;
        _oldStatus = CurrentOrder.Status;
        Status = CurrentOrder.Status;
    }
    public void StatusChanged(object sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName != nameof(Status)) return;
        IsStatusChanged = Status != _oldStatus;
    }

    [RelayCommand]
    async Task ChangeStatusAsync()
    {
        try
        {
            IsBusy = true;
            await _service.ChangeStatusAsync(CurrentOrder.ID, Status);
            await Toast.Make(GeneralAlerts.ORDER_STATUS_CHANGED, ToastDuration.Short).Show();
            _oldStatus = Status;
            IsStatusChanged = false;
        }
        catch (Exception)
        {
            //await Shell.Current.DisplayAlert("ERROR", "Could not change order status!", "OK");
            await Toast.Make(GeneralAlerts.NO_CONNECTION, ToastDuration.Short).Show();
        }
        finally
        {
            IsBusy = false;
        }
    }
}
