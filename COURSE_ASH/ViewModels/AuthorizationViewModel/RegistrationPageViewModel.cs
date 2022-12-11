using COURSE_ASH.Services.AccountServices;

namespace COURSE_ASH.ViewModels.AuthorizationViewModel;

public partial class RegistrationPageViewModel: BaseViewModel
{
    readonly RegistrationService _service;

    [ObservableProperty]
    public string login;

    [ObservableProperty]
    string password;

    [ObservableProperty]
    bool isPasswordVisible;

    [ObservableProperty]
    string confirmPassword;

    [ObservableProperty]
    string alert;

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
