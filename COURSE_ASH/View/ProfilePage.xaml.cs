namespace COURSE_ASH.View;

public partial class ProfilePage : ContentPage
{
    public ProfilePage(ProfilePageViewModel viewModel)
    {
        InitializeComponent();
        BindingContext=viewModel;
    }
}