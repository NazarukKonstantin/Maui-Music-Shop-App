namespace COURSE_ASH.ViewModels.UserViewModels;

[QueryProperty(nameof(CurrentOrder),nameof(CurrentOrder))]
public partial class OrderPageViewModel : BaseViewModel
{
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(CanBeCancelled))]
    Order _currentOrder;

    public bool CanBeCancelled => Order
        .CanBeCancelled(CurrentOrder?.Status ?? null);

    private readonly OrderService _orderService;
    
    public OrderPageViewModel(OrderService orderService)
    {
        _orderService = orderService;
    }

    [RelayCommand]
    async Task RequestCancellationAsync()
    {
        try
        {
            IsBusy = true;
            CurrentOrder = await _orderService
                .ChangeStatusAsync(CurrentOrder.ID, OrderStatus.CANCELLATION_REQUESTED);
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
