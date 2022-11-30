namespace COURSE_ASH.ViewModel;

public partial class LogInPageViewModel : BaseViewModel
{
    readonly LogInService _logInService;

    [ObservableProperty]
    string login;

    [ObservableProperty]
    string password;

    [ObservableProperty]
    bool isSuccessful;

    [ObservableProperty]
    bool isLoginOrPasswordWrong;

    [ObservableProperty]
    string errorMessage;

    public LogInPageViewModel(LogInService logInService)
    {
        _logInService = logInService;
    }

    [RelayCommand]
    public async Task SignIn()
    {
        IsBusy = true;
        AccountConfirmator accountConf = await _logInService.LogInAsync(Login, Password);
        if (accountConf.Alert == AccountAlerts.SUCCESS) isSuccessful = true;
        else isSuccessful = false;
        if (isSuccessful)
        {
            App.CurrentAccount.Login = Login;
            App.CurrentAccount.IsLoggedIn = true;
            IsLoginOrPasswordWrong = false;
            await Shell.Current.GoToAsync($"..",
            new Dictionary<string, object>
            {
                ["IsSignedIn"] = isSuccessful
            });
        }
        else
        {
            ErrorMessage = accountConf.Alert;
            IsLoginOrPasswordWrong = true;
            Password = string.Empty;
        }
        IsBusy = false;
    }
    [RelayCommand]
    Task SignUp()
    {
        Login = string.Empty;
        Password = string.Empty;
        IsLoginOrPasswordWrong = false;
        IsSuccessful = false;
        return Shell.Current.GoToAsync(nameof(RegistrationPage), true);
    }

    [RelayCommand]
    public async Task GoBack()
    {
        await Shell.Current.GoToAsync($"..",
        new Dictionary<string, object>
        {
            ["IsSignedIn"] = isSuccessful
        });
    }
}
