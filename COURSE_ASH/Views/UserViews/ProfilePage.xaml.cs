namespace COURSE_ASH.Views.UserViews;

public partial class ProfilePage : ContentPage
{
    public ProfilePage(ProfilePageViewModel viewModel)
    {
        InitializeComponent();
        BindingContext=viewModel;
    }
}