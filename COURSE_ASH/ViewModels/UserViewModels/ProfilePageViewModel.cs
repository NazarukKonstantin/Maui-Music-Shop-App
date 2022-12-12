namespace COURSE_ASH.ViewModels.UserViewModels;

[QueryProperty(nameof(ImageLink),nameof(ImageLink))]
public partial class ProfilePageViewModel : BaseViewModel
{
    private readonly PasswordChangingService _passwordChangingService;
    private FileResult _image = null;

    [ObservableProperty]
    private string _currentLogin;

    [ObservableProperty]
    private string _oldPassword;

    [ObservableProperty]
    private string _newPassword;

    [ObservableProperty]
    private string _confirmPassword;

    [ObservableProperty]
    private string _imageLink;

    [ObservableProperty]
    private string _alert;
    
    public ProfilePageViewModel(PasswordChangingService passwordChangingService)
    {
        _passwordChangingService = passwordChangingService;
        RefreshAsync();
    }

    [RelayCommand]
    async void GoToOrderHistoryPageAsync()
    {
        await Shell.Current.GoToAsync($"{nameof(OrderHistoryPage)}",
            new Dictionary<string, object>
            {
                ["CurrentLogin"] = CurrentLogin,
            });
    }


    [RelayCommand]
    async Task ChangePasswordAsync()
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

    [RelayCommand]
    async Task PickImage()
    {
        _image = await MediaPicker.PickPhotoAsync();
        if (_image is null) return;
        ImageLink = _image.FullPath;
    }

    public void RefreshAsync()
    {
        CurrentLogin = App.CurrentLogin;
        IsSuccessful = false;
        IsFailed = false;
    }
}
