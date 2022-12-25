using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;

namespace COURSE_ASH.ViewModels.UserViewModels;

[QueryProperty(nameof(TotalPrice), nameof(TotalPrice))]
[QueryProperty(nameof(BillingAddress), nameof(BillingAddress))]
[QueryProperty(nameof(Products), nameof(Products))]
public partial class CheckoutPageViewModel : BaseViewModel
{
    private readonly OrderService _orderService;
    private readonly CartService _cartService;

    [ObservableProperty]
    private ObservableCollection<CartProduct> _products;

    [ObservableProperty]
    private string _billingAddress;

    [ObservableProperty]
    private string _email;

    [ObservableProperty]
    private double _totalPrice;

    [ObservableProperty]
    private bool _isNotEmpty = false;

    public CheckoutPageViewModel(OrderService orderService, CartService cartService)
    {
        _cartService = cartService;
        _orderService = orderService;
    }

    [RelayCommand]
    async Task ConfirmAsync()
    {
        IsBusy = true;
        try
        {
            if (string.IsNullOrEmpty(BillingAddress))
            {
                await Toast.Make(AddressAlerts.WRONG_ADDRESS, ToastDuration.Long).Show();
                IsBusy = false;
                return;
            }
            if (string.IsNullOrEmpty(Email) || !Order.IsEmail(Email))
            {
                await Toast.Make(AddressAlerts.WRONG_EMAIL, ToastDuration.Long).Show();
                IsBusy = false;
                return;
            }
            await _orderService.CheckoutAsync(App.CurrentLogin,
                Products.ToList(),
                BillingAddress,
                TotalPrice);

            await _cartService.ClearStorageCartAsync(App.CurrentLogin);

            await Shell.Current.DisplayAlert("SUCCESS!",
                "You order awaits confirmation\n" +
                "You can follow the status update in your account page",
                "OK");
            await Shell.Current.GoToAsync($"..");
        }
        catch
        {
            //await Shell.Current.DisplayAlert("ERROR", "Could not make an order!", "OK");
            //await Shell.Current.DisplayAlert("ERROR", "Could not clear the cart!", "OK");
            await Toast.Make(GeneralAlerts.NO_CONNECTION, ToastDuration.Short).Show();
        }
        finally
        {
            IsBusy = false;
        }
    }

    private void CheckEmpty(object sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(IsNotEmpty)) return;
        IsNotEmpty = !(string.IsNullOrEmpty(BillingAddress));
    }
    [RelayCommand]
    async Task GoBackAsync()
    {
        await Shell.Current.GoToAsync("..");
    }
}
