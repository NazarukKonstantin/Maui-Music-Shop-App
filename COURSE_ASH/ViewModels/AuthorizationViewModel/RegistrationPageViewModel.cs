using COURSE_ASH.Services.AccountServices;

namespace COURSE_ASH.ViewModels.AuthorizationViewModel;

public partial class RegistrationPageViewModel: BaseViewModel
{
    private readonly RegistrationService _service;

    [ObservableProperty]
    private string _login;

    [ObservableProperty]
    private string _password;

    [ObservableProperty]
    private bool _isPasswordVisible;

    [ObservableProperty]
    private string _confirmPassword;

    [ObservableProperty]
    private string _alert;

    public RegistrationPageViewModel(RegistrationService service)
    {
        _service=service;
    }

    [RelayCommand]
    async Task CreateAccount()
    {
        IsBusy = true;
        AccountState accountState = await _service.RegisterUserAsync(Login, Password, ConfirmPassword);
        if (accountState.Alert != AccountAlerts.SUCCESS)
        {
            IsSuccessful = false;
            Alert = accountState.Alert;
        }
        else IsSuccessful=true;
        IsBusy = false;
    }

    [RelayCommand]
    async Task GoBackAsync()
    {
        await Shell.Current.GoToAsync("..");
    }
}
