namespace COURSE_ASH.Views.AuthorizationViews;

public partial class RegistrationPage : ContentPage
{
	public RegistrationPage(RegistrationPageViewModel viewModel)
	{
		InitializeComponent();
		BindingContext=viewModel;
	}

    private void VisibilityTapped(object sender, EventArgs e)
    {
        if (PassEntry.IsPassword)
        {
            PassEntry.IsPassword=false;
            VisibilityOnImg.IsVisible=false;
            VisibilityOffImg.IsVisible=true;
            ConfirmPassEntry.IsPassword=false;
            ConfirmVisibilityOnImg.IsVisible =false;
            ConfirmVisibilityOffImg.IsVisible=true;
        }
        else
        {
            PassEntry.IsPassword=true;
            VisibilityOnImg.IsVisible=true;
            VisibilityOffImg.IsVisible=false;
            ConfirmPassEntry.IsPassword=true;
            ConfirmVisibilityOnImg.IsVisible =true;
            ConfirmVisibilityOffImg.IsVisible=false;
        }
    }
}