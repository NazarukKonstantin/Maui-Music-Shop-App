using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;

namespace COURSE_ASH.ViewModels.AdminViewModels;

[QueryProperty (nameof(CurrentOrder),nameof(CurrentOrder))]
public partial class OrderStatusPageViewModel : BaseViewModel
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
    public OrderStatusPageViewModel(OrderService orderService)
    {
        _service = orderService;

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
        //if (Status == _oldStatus) IsStatusChanged = false;
        //else IsStatusChanged = true;
        IsStatusChanged = Status != _oldStatus;
    }

    [RelayCommand]
    async Task ChangeStatusAsync()
    {
        IsBusy = true;
        await _service.ChangeStatusAsync(CurrentOrder.ID, Status);
        await Toast.Make(GeneralAlerts.ORDER_STATUS_CHANGED, ToastDuration.Short).Show();
        _oldStatus = Status;
        IsStatusChanged = false;
        IsBusy = false;
    }
}
