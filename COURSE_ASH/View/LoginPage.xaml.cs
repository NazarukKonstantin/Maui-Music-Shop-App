using COURSE_ASH.ViewModel;

namespace COURSE_ASH.View;

public partial class LoginPage : ContentPage
{
    public LoginPage(LoginPageViewModel viewModel)
    {
        InitializeComponent();
        BindingContext=viewModel;
    }

    //[RelayCommand]
    //void ChangePasswordVisibility()
    //{
    //	if(VisibilityImg.Source.Equals("visibility_on"))
    //	{
    //		VisibilityImg.Source="visibility_off";
    //		PassEntry.IsPassword=true;
    //	}
    //	else
    //	{
    //           VisibilityImg.Source="visibility_on";
    //           PassEntry.IsPassword=false;
    //       }
    //}
}