namespace COURSE_ASH.ViewModels.UserViewModels;

[QueryProperty(nameof(CurrentOrder),nameof(CurrentOrder))]
public partial class OrderPageViewModel : BaseViewModel
{
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsCancelable))]
    Order _currentOrder;

    public bool IsCancelable => Order
        .CanBeCancelled(CurrentOrder?.Status ?? null);

    private readonly OrderService _orderService;
    
    public OrderPageViewModel(OrderService orderService)
    {
        _orderService = orderService;
    }

    [RelayCommand]
    async Task RequestCancellationAsync()
    {
        IsBusy = true;
        CurrentOrder = await _orderService
            .ChangeStatusAsync(CurrentOrder.ID, OrderStatus.CANCELLATION_REQUESTED);
        IsBusy = false;
    }
}
