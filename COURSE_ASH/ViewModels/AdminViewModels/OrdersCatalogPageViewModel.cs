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
        Orders = (await _service.GetAllOrdersAsync()).ToObservableCollection();
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
