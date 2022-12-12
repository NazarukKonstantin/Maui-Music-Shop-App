namespace COURSE_ASH.ViewModels.AuthorizationViewModels;

public partial class LogInPageViewModel : BaseViewModel
{
    readonly LogInService _logInService;

    [ObservableProperty]
    private string _login;

    [ObservableProperty]
    private string _password;

    [ObservableProperty]
    private bool _isPasswordVisible;

    [ObservableProperty]
    private string _alert;

    public LogInPageViewModel(LogInService logInService)
    {
        _logInService = logInService;
    }

    [RelayCommand]
    public async Task LogIn()
    {
        IsBusy = true;
        AccountState accountState = await _logInService.LogInUserAsync(Login, Password);
        if (accountState.Alert == AccountAlerts.SUCCESS)
        {
            IsSuccessful = true;
            IsFailed = false;
            App.CurrentLogin = accountState.Login;
            if (accountState.Role == Roles.Admin)
                App.Current.MainPage = new AdminShell();
            else
                App.Current.MainPage = new AppShell();
        }
        else
        {
            IsSuccessful = false;
            Alert = accountState.Alert;
            IsFailed = true;
            Password = string.Empty;
        }
        IsBusy = false;
    }

    [RelayCommand]
    async Task SignUp()
    {
        IsBusy = true;
        Login = string.Empty;
        Password = string.Empty;
        IsFailed = false;
        IsSuccessful = false;
        await Shell.Current.GoToAsync(nameof(RegistrationPage));
        IsBusy = false;
    }
}