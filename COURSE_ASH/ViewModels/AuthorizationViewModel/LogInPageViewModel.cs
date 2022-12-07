using COURSE_ASH.Services.AccountServices;

namespace COURSE_ASH.ViewModels.AuthorizationViewModel;

public partial class LogInPageViewModel : BaseViewModel
{
    readonly LogInService _logInService;

    [ObservableProperty]
    string login;

    [ObservableProperty]
    string password;

    [ObservableProperty]
    bool isPasswordVisible;

    [ObservableProperty]
    string errorMessage;

    public LogInPageViewModel(LogInService logInService)
    {
        _logInService = logInService;
    }

    [RelayCommand]
    public async Task LogIn()
    {
        IsBusy = true;
        AccountData accountConf = await _logInService.LogInAsync(Login, Password);
        if (accountConf.Alert == AccountAlerts.SUCCESS)
        {
            IsSuccessful = true;
            IsFailed = false;
            App.CurrentLogin = accountConf.Login;
            if (accountConf.Role == Roles.Admin)
                App.Current.MainPage = new AdminShell();
            else
                App.Current.MainPage = new AppShell();
        }
        else
        {
            IsSuccessful = false;
            ErrorMessage = accountConf.Alert;
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
