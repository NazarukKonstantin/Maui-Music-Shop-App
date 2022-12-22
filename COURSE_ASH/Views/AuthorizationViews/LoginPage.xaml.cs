using Microsoft.Maui.Platform;

namespace COURSE_ASH.Views.AuthorizationViews;

public partial class LoginPage : ContentPage
{
    private readonly LogInPageViewModel _viewModel;
    public LoginPage(LogInPageViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = viewModel;
    }
    private void VisibilityChanged(object sender, EventArgs e)
    {
        PassEntry.IsPassword = !PassEntry.IsPassword;
        VisibilityOnImg.IsVisible = !VisibilityOnImg.IsVisible;
        VisibilityOffImg.IsVisible = !VisibilityOffImg.IsVisible;
    }

    private void LoginEntryCompleted(object sender, EventArgs e)
    {
        LoginEntry.Unfocus();
        PassEntry.Focus();
    }

    private async void PasswordEntryCompleted(object sender, EventArgs e)
    {
#if ANDROID
        if (Platform.CurrentActivity.CurrentFocus != null)
            Platform.CurrentActivity
                .HideKeyboard(Platform.CurrentActivity.CurrentFocus);
#endif

        PassEntry.Unfocus();

        LoginButton.Focus();
        await _viewModel.LogInAsync();
    }
}