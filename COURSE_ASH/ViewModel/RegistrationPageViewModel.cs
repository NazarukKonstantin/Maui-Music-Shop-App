namespace COURSE_ASH.ViewModel;

public partial class RegistrationPageViewModel: BaseViewModel
{
    readonly AccountService accountService;

    [ObservableProperty]
    public string login;

    [ObservableProperty]
    string password;

    [ObservableProperty]
    string repeatPassword;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsNotSuccessful))]
    bool isSuccessful;

    public bool IsNotSuccessful => !isSuccessful;

    [ObservableProperty]
    string alert;

    public RegistrationPageViewModel(AccountService accountService)
    {
        this.accountService = accountService;
    }

    [RelayCommand]
    async Task CreateAccount()
    {
        IsBusy = true;
        AccountConfirmator accountConf = await accountService.CreateAccountAsync(Login, Password, RepeatPassword);
        if (accountConf.Alert == AccountAlerts.SUCCESS) IsSuccessful = true;
        if (IsSuccessful)
        {
        }
        else
        {
            Alert = accountConf.Alert;
        }
        IsBusy = false;
    }

    [RelayCommand]
    Task GoBack()
    {
        return Shell.Current.GoToAsync("..");
    }
}
