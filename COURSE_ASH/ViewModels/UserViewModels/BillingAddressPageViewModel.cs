namespace COURSE_ASH.ViewModels.UserViewModels;

public partial class BillingAddressPageViewModel : BaseViewModel
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

    public BillingAddressPageViewModel(BillingAddressService addressService)
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
    }

    [RelayCommand]
    private async Task SuggestPostCode()
    {
        PostalCode = await _addressService.GetPostalCodeAsync(Country, City, Street, BuildingNumber);
    }

    private async Task GoBackAsync(string address)
    {
        await Shell.Current.GoToAsync($"..",
            new Dictionary<string, object>
            {
                ["Address"] = address
            });
    }
}
