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
    private string _phoneNumber;

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

        if (string.IsNullOrEmpty(BillingAddress))
        {
            await Toast.Make(AddressAlerts.WRONG_ADDRESS, ToastDuration.Long).Show();
            IsBusy = false;
            return;
        }
        if (string.IsNullOrEmpty(PhoneNumber) || !Order.IsPhoneNumber(PhoneNumber))
        {
            await Toast.Make(AddressAlerts.WRONG_NUMBER, ToastDuration.Long).Show();
            IsBusy = false;
            return;
        }
        try
        {
            await _orderService.CheckoutAsync(App.CurrentLogin,
                Products.ToList(),
                BillingAddress,
                TotalPrice);
        }
        catch (Exception)
        {
            await Shell.Current.DisplayAlert("ERROR", "Could not make an order!", "OK");
        }
        try
        {
            await _cartService.ClearStorageCartAsync(App.CurrentLogin);
        }
        catch(Exception)
        {
            await Shell.Current.DisplayAlert("ERROR", "Could not clear the cart!", "OK");
        }

        await Shell.Current.DisplayAlert("SUCCESS!",
            "You order awaits confirmation\n" +
            "You can follow the status update in your account page",
            "OK");
        await Shell.Current.GoToAsync($"..");

        IsBusy = false;
    }

    private void CheckEmpty(object sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(IsNotEmpty)) return;
        IsNotEmpty=!(string.IsNullOrEmpty(BillingAddress));
    }
    [RelayCommand]
    async Task EnterAddressAsync()
    {
        await Shell.Current.GoToAsync($"{nameof(BillingAddressPage)}");
    }
}
