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

    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        Focus();
        base.OnNavigatedTo(args);
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
        Focus();
    }

    private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
    {
        _viewModel.Login = "testUser";
        _viewModel.Password = "12345678";
    }

    private void TapGestureRecognizer_Tapped_1(object sender, EventArgs e)
    {
        _viewModel.Login = "testUser1";
        _viewModel.Password = "12345678";
    }
}