using COURSE_ASH.Models;

namespace COURSE_ASH.ViewModels;

[QueryProperty(nameof(ImageLink), nameof(ImageLink))]
[QueryProperty(nameof(IsNotAdmin),nameof(IsNotAdmin))]
[QueryProperty(nameof(ImageRotation), nameof(ImageRotation))]
[QueryProperty(nameof(ImageScale), nameof(ImageScale))]
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
    double _imageRotation = 0;

    [ObservableProperty]
    double _imageScale = 1;

    [ObservableProperty]
    private string _alert;

    [ObservableProperty]
    private bool _isNotAdmin;
    
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
    public async Task ChangePasswordAsync()
    {
        try
        {
            IsBusy = true;
            IsSuccessful = false;
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
        catch
        {
            IsSuccessful = false;
            IsFailed = true;
            //await Shell.Current.DisplayAlert("ERROR", "Could not change password!", "OK");
            await Toast.Make(GeneralAlerts.NO_CONNECTION, ToastDuration.Short).Show();
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

    [RelayCommand]
    async Task SaveImageAsync()
    {
        try
        {
            await AccountService.RecordNewImage(_image, ImageRotation, ImageScale, CurrentLogin);
            await Toast.Make(GeneralAlerts.IMAGE_CHANGED, ToastDuration.Short).Show();
        }
        catch
        {
            await Toast.Make(GeneralAlerts.NO_CONNECTION, ToastDuration.Short).Show();
        }

    }

    [RelayCommand]
    public async void RefreshAsync()
    {
        CurrentLogin = App.CurrentLogin;
        AccountData currentAccount = await DataStorageService<AccountData>
                    .GetItemByAsync(nameof(AccountData.CurrentLogin), CurrentLogin);
        ImageLink = currentAccount.ImageLink;
        ImageRotation = currentAccount.ImageRotation;
        ImageScale = currentAccount.ImageScale;
        if (string.IsNullOrEmpty(ImageLink))
            ImageLink = App.Current.UserAppTheme == AppTheme.Dark ? 
                Icons.WaltuhWhite : Icons.WaltuhBlack; 
        IsSuccessful = false;
        IsFailed = false;
    }

    [RelayCommand]
    async Task GoBackAsync()
    {
        await Shell.Current.GoToAsync("..",true,
            new Dictionary<string, object>
            {
                ["ProfileImageLink"]=ImageLink,
                ["ProfileImageRotation"]=ImageRotation,
                ["ProfileImageScale"]=ImageScale,
            });
    }
}
