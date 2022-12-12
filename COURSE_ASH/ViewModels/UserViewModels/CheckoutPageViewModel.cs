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
    private string _email;

    [ObservableProperty]
    private string _billingAddress;

    [ObservableProperty]
    private double totalPrice;

    [ObservableProperty]
    private bool isNotEmpty = false;

    public CheckoutPageViewModel(OrderService orderService, CartService cartService)
    {
        _cartService = cartService;
        _orderService = orderService;
    }

    private void CheckEmpty(object sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(IsNotEmpty)) return;
        IsNotEmpty=!(string.IsNullOrEmpty(BillingAddress)||
                     string.IsNullOrEmpty(Email));
    }
    [RelayCommand]
    async Task EnterAddressAsync()
    {
        await Shell.Current.GoToAsync($"{nameof(BillingAddressPage)}");
    }

    [RelayCommand]
    async Task ConfirmAsync()
    {
        IsBusy = true;

        //await _orderService.CheckoutAsync(App.UserName, 
        //    Products.ToList(), 
        //    await _addressService.GetFormattedAddressAsync(Country, City, Street, (int)BuildingNumber),
        //    TotalPrice);
        await _cartService.ClearStorageCartAsync(App.CurrentLogin);

        await Shell.Current.DisplayAlert("SUCCESSFUL!",
            $"Order is in confirmation\nCheck out your profile page for delivery details\n",
            "OK");
        await Shell.Current.GoToAsync($"..");
        IsBusy = false;
    }
}
