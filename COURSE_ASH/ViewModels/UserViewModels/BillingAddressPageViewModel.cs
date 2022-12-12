namespace COURSE_ASH.ViewModels.UserViewModels;

public partial class BillingAddressPageViewModel : BaseViewModel
{
    public List<string> Countries { get; } = BillingAddressService.GetCountries().ToList();

    readonly BillingAddressService _addressService;

    [ObservableProperty]
    ObservableCollection<CartProduct> _products;

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
    private string _postCode;

    [ObservableProperty]
    bool isNotEmpty = false;

    public BillingAddressPageViewModel(BillingAddressService addressService)
    {
        _addressService = addressService;
    }

    [RelayCommand]
    async Task CheckAddress()
    {
        IsBusy = true;
        Alert = await _addressService.ValidateAddressAsync(Country, City, Street, BuildingNumber, PostCode);
        if (Alert != AccountAlerts.SUCCESS)
        {
            IsFailed = true;
            IsBusy = false;
            return;
        }
        IsFailed = false;
        await GoBackAsync(_addressService.GetFormattedAddress(Country, City, Street,
            BuildingNumber, ApartmentNumber, PostCode));
    }

    [RelayCommand]
    async Task SuggestPostCode()
    {
        PostCode = await _addressService.GetPostCode(Country, City, Street, BuildingNumber);
    }

    [RelayCommand]
    async Task GoBackAsync(string address)
    {
        await Shell.Current.GoToAsync($"..",
            new Dictionary<string, object>
            {
                ["Address"] = address
            });
    }
}
