namespace COURSE_ASH.ViewModels;

[QueryProperty(nameof(ImageLink),nameof(ImageLink))]
public partial class ProfilePageViewModel : BaseViewModel
{
    private readonly PasswordChangingService _service;
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
    
    public ProfilePageViewModel(PasswordChangingService service)
    {
        _service = service;
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
        try
        {
            IsBusy = true;
            AccountState accountState = await _service
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
        }
        catch (Exception)
        {
            await Shell.Current.DisplayAlert("ERROR", "Could not change password!", "OK");
        }
        finally
        {
            IsBusy = false;
        }
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
