namespace COURSE_ASH.Views;

public partial class ProfilePage : ContentPage
{
    public ProfilePage(ProfilePageViewModel viewModel)
    {
        InitializeComponent();
        BindingContext=viewModel;
    }
}