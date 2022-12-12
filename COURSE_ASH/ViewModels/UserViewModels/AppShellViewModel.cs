namespace COURSE_ASH.ViewModels.UserViewModels;

public partial class AppShellViewModel : BaseViewModel
{
    [ObservableProperty]
    private string _currentLogin;

    public AppShellViewModel()
    {
        CurrentLogin = App.CurrentLogin;
    }

    [RelayCommand]
    void SignOut()
    {
        CurrentLogin= null;
        App.CurrentLogin = null;
        App.Current.MainPage = new AuthorizationShell();
    }
}
