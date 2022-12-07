using COURSE_ASH.Services.AccountServices;

namespace COURSE_ASH.ViewModels.AuthorizationViewModel;

public partial class RegistrationPageViewModel: BaseViewModel
{
    readonly AccountService _accountService;

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

    public RegistrationPageViewModel(AccountService accountService)
    {
        _accountService = accountService;
    }

    [RelayCommand]
    async Task CreateAccount()
    {
        IsBusy = true;
        AccountData accountConf = await _accountService.CreateAccountAsync(Login, Password, ConfirmPassword);
        if (accountConf.Alert != AccountAlerts.SUCCESS)
        {
            IsSuccessful = false;
            Alert = accountConf.Alert;
        }
        else IsSuccessful=true;
        IsBusy = false;
    }
}
