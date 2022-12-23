namespace COURSE_ASH.ViewModels.UserViewModels;

public partial class BillingAddressPopupViewModel : BaseViewModel
{
    public List<string> Countries { get; } = BillingAddressService.GetCountries().ToList();

    readonly BillingAddressService _addressService;


    [ObservableProperty]
    private string _alert;

    [ObservableProperty]
    private string _country;
    [ObservableProperty]
    private string _city;
    [ObservableProperty]
    private string _street;
    [ObservableProperty]
    private int? _buildingNumber;
    [ObservableProperty]
    private int? _apartmentNumber;
    [ObservableProperty]
    private string _postalCode;

    [ObservableProperty]
    bool isNotEmpty = false;

    public BillingAddressPopupViewModel(BillingAddressService addressService)
    {
        _addressService = addressService;
    }

    [RelayCommand]
    private async Task CheckAddress()
    {
        IsBusy = true;
        Alert = await _addressService.CheckAddressAsync(Country, City, Street, BuildingNumber, PostalCode);
        if (Alert != AddressAlerts.SUCCESS)
        {
            IsFailed = true;
            IsBusy = false;
            return;
        }
        IsFailed = false;
        await GoBackAsync(_addressService.GetFormattedAddress(Country, City, Street,
            BuildingNumber, ApartmentNumber, PostalCode));
        IsBusy = false;
    }

    private async Task GoBackAsync(string address)
    {
        await Shell.Current.GoToAsync($"{nameof(CheckoutPage)}",
            new Dictionary<string, object>
            {
                ["BillingAddress"] = address
            });
    }
}
