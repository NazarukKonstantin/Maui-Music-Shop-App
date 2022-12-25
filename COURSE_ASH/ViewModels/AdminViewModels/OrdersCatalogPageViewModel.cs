namespace COURSE_ASH.ViewModels.AdminViewModels;

public partial class OrdersCatalogPageViewModel : BaseViewModel, IRefreshable
{
    [ObservableProperty]
    private ObservableCollection<Order> _orders;

    private readonly OrderService _service;
    public OrdersCatalogPageViewModel(OrderService orderService)
    {
        _service = orderService;

        RefreshAsync();
    }

    [RelayCommand]
    public async void RefreshAsync()
    {
        try
        {
            IsRefreshing = true;
            IsBusy = true;
            Orders = (await _service.GetAllOrdersAsync()).ToObservableCollection();
        }
        catch
        {
            //await Shell.Current.DisplayAlert("ERROR", "Could not load orders!", "OK");
            await Toast.Make(GeneralAlerts.NO_CONNECTION, ToastDuration.Short).Show();
        }
        finally
        {
            IsRefreshing = false;
            IsBusy = false;
        }
    }

}
