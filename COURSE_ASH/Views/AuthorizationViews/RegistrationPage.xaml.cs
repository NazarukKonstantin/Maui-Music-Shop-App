using Microsoft.Maui.Platform;

namespace COURSE_ASH.Views.AuthorizationViews;

public partial class RegistrationPage : ContentPage
{
    private readonly RegistrationPageViewModel _viewModel;
	public RegistrationPage(RegistrationPageViewModel viewModel)
	{
		InitializeComponent();
        _viewModel = viewModel;
		BindingContext=viewModel;
	}

    private void VisibilityTapped(object sender, EventArgs e)
    {
        PassEntry.IsPassword = !PassEntry.IsPassword;
        VisibilityOnImg.IsVisible = !VisibilityOnImg.IsVisible;
        VisibilityOffImg.IsVisible = !VisibilityOffImg.IsVisible;
        Focus();
    }

    private void LoginEntryCompleted(object sender, EventArgs e)
    {
        if (LoginEntry.Focus())
        {
            LoginEntry.Unfocus();
            PassEntry.Focus();
        }
    }

    private void PasswordEntryCompleted(object sender, EventArgs e)
    {
        if (PassEntry.Focus())
        {
            PassEntry.Unfocus();
            ConfirmPassEntry.Focus();
        }
    }

    private async void ConfirmPasswordEntryCompleted(object sender, EventArgs e)
    {
#if ANDROID
        if (Platform.CurrentActivity.CurrentFocus != null)
            Platform.CurrentActivity
                .HideKeyboard(Platform.CurrentActivity.CurrentFocus);
#endif
        if (ConfirmPassEntry.Focus())
        {
            ConfirmPassEntry.Unfocus();
        }
        SignUpButton.Focus();
        await _viewModel.CreateAccountAsync();
    }
}