namespace COURSE_ASH.ViewModels.AdminViewModel
{
    public partial class AdminShellViewModel:ObservableObject
    {
        [ObservableProperty]
        private string _currentLogin;

        public AdminShellViewModel()
        {
            CurrentLogin = App.CurrentLogin;
        }

        [RelayCommand]
        void SignOut()
        {
            App.Current.MainPage = new AuthorizationShell();
        }
    }
}
