using Microsoft.Maui.Platform;

namespace COURSE_ASH.Views;

public partial class ProfilePage : ContentPage
{
    private readonly ProfilePageViewModel _viewModel;
    public ProfilePage(ProfilePageViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext=viewModel;
    }

    private void VisibilityTapped(object sender, EventArgs e)
    {
        OldPassEntry.IsPassword = !OldPassEntry.IsPassword;
        NewPassEntry.IsPassword = !NewPassEntry.IsPassword;
        ConfirmNewPassEntry.IsPassword = !ConfirmNewPassEntry.IsPassword;

        OldPassVisibilityOnImg.IsVisible = !OldPassVisibilityOnImg.IsVisible;
        OldPassVisibilityOffImg.IsVisible = !OldPassVisibilityOffImg.IsVisible;
        NewPassVisibilityOnImg.IsVisible = !NewPassVisibilityOnImg.IsVisible;
        NewPassVisibilityOffImg.IsVisible = !NewPassVisibilityOffImg.IsVisible;
        ConfirmVisibilityOnImg.IsVisible = !ConfirmVisibilityOnImg.IsVisible;
        ConfirmVisibilityOffImg.IsVisible = !ConfirmVisibilityOffImg.IsVisible;
    }

    private void OldPasswordEntryCompleted(object sender, EventArgs e)
    {
        OldPassEntry.Unfocus();
        NewPassEntry.Focus();
    }

    private void NewPasswordEntryCompleted(object sender, EventArgs e)
    {
        NewPassEntry.Unfocus();
        ConfirmNewPassEntry.Focus();
    }

    private async void ConfirmPasswordEntryCompleted(object sender, EventArgs e)
    {
#if ANDROID
        if (Platform.CurrentActivity.CurrentFocus != null)
            Platform.CurrentActivity
                .HideKeyboard(Platform.CurrentActivity.CurrentFocus);
#endif
        ConfirmNewPassEntry.Unfocus();
        ConfirmPassButton.Focus();
        await _viewModel.ChangePasswordAsync();
    }

    private void ChangePassButton_Pressed(object sender, EventArgs e)
    {
        ChangePassButton.IsVisible = !ChangePassButton.IsVisible;
        ChangePasswordFrame.IsVisible =!ChangePasswordFrame.IsVisible;
        RotationSlider.IsVisible = !RotationSlider.IsVisible;
        ScaleSlider.IsVisible = !ScaleSlider.IsVisible;
        SaveImageButton.IsVisible = !SaveImageButton.IsVisible;
        ConfirmPassButton.IsVisible = !ConfirmPassButton.IsVisible;
    }

    private void ConfirmPassButton_Clicked(object sender, EventArgs e)
    {
        ChangePassButton.IsVisible = !ChangePassButton.IsVisible;
        ChangePasswordFrame.IsVisible = !ChangePasswordFrame.IsVisible;
        RotationSlider.IsVisible = !RotationSlider.IsVisible;
        ScaleSlider.IsVisible = !ScaleSlider.IsVisible;
        SaveImageButton.IsVisible = !SaveImageButton.IsVisible;
        ConfirmPassButton.IsVisible = !ConfirmPassButton.IsVisible;
        OldPassEntry.Text = String.Empty;
        NewPassEntry.Text = String.Empty;
        ConfirmNewPassEntry.Text = String.Empty;
    }
}