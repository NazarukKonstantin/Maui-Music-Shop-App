namespace COURSE_ASH.Views.AuthorizationViews;

public partial class LoginPage : ContentPage
{
    public LoginPage(LogInPageViewModel viewModel)
    {
        InitializeComponent();
        BindingContext=viewModel;
    }

    private void VisibilityTapped(object sender, EventArgs e)
    {
        if(PassEntry.IsPassword)
        {
            PassEntry.IsPassword=false;
            VisibilityOnImg.IsVisible=false;
            VisibilityOffImg.IsVisible=true;
        }
        else
        {
            PassEntry.IsPassword=true;
            VisibilityOnImg.IsVisible=true;
            VisibilityOffImg.IsVisible=false;
        }
    }
}