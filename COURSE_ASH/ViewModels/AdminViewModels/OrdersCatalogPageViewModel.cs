namespace COURSE_ASH.ViewModels.AdminViewModels;

public partial class OrdersCatalogPageViewModel : BaseViewModel
{
    [ObservableProperty]
    ObservableCollection<Order> _orders;

    OrderService _service;
    public OrdersCatalogPageViewModel(OrderService orderService)
    {
        _service = orderService;

        RefreshAsync();
    }
    public async void RefreshAsync()
    {
        try
        {
            IsRefreshing = true;
            IsBusy = true;
            Orders = (await _service.GetAllOrdersAsync()).ToObservableCollection();
        }
        catch(Exception)
        {
            await Shell.Current.DisplayAlert("ERROR", "Could not load orders!", "OK");
        }
        finally
        {
            IsRefreshing = false;
            IsBusy = false;
        }
    }

    [RelayCommand]
    async Task GoToOrder(Order order)
    {
        await Shell.Current.GoToAsync($"{nameof(OrderStatusPage)}",
            new Dictionary<string, object>
            {
                ["CurrentOrder"] = order
            });
    }
}
