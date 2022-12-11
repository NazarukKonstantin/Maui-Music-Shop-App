namespace COURSE_ASH.ViewModels.UserViewModels;

public partial class ProfilePageViewModel : BaseViewModel
{
    private readonly PasswordChangingService _passwordChangingService;

    [ObservableProperty]
    private string _currentLogin;

    [ObservableProperty]
    private string _oldPassword;

    [ObservableProperty]
    private string _newPassword;

    [ObservableProperty]
    private string _confirmPassword;

    [ObservableProperty]
    private string _alert;
    
    public ProfilePageViewModel(PasswordChangingService passwordChangingService)
    {
        _passwordChangingService = passwordChangingService;
        RefreshAsync();
    }

    [RelayCommand]
    async void GoToOrdersAsync()
    {
        await Shell.Current.GoToAsync($"{nameof(OrderHistoryPage)}",
            new Dictionary<string, object>
            {
                ["CurrentLogin"] = CurrentLogin,
            });
    }


    [RelayCommand]
    public async Task CahngePassword()
    {
        IsBusy = true;
        AccountState accountState = await _passwordChangingService
            .ChangePasswordAsync(App.CurrentLogin, OldPassword, NewPassword, ConfirmPassword);
        if (accountState.Alert == AccountAlerts.SUCCESS)
        {
            IsSuccessful = true;
            IsFailed = false;
        }
        else
        {
            IsSuccessful = false;
            Alert = accountState.Alert;
            IsFailed = true;
        }
        OldPassword = string.Empty;
        NewPassword = string.Empty;
        ConfirmPassword = string.Empty;

        IsBusy = false;
    }
    public void RefreshAsync()
    {
        CurrentLogin = App.CurrentLogin;

        IsSuccessful = false;
        IsFailed = false;
    }
}
