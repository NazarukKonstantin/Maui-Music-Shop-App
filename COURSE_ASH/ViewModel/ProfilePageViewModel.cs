namespace COURSE_ASH.ViewModel;

[QueryProperty(nameof(IsLoggedIn), nameof(IsLoggedIn))]
public partial class ProfilePageViewModel : BaseViewModel
{
    [ObservableProperty]
    Account currentAccount;

    Image profilePic = new()
    {
        Source=Icons.AddUser,
    };

    [ObservableProperty]
    string userName;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsNotLoggedIn))]
    bool isLoggedIn;

    public bool IsNotLoggedIn => !IsLoggedIn;

    public ProfilePageViewModel()
    {
        IsLoggedIn = App.CurrentAccount.IsLoggedIn;
        if (IsLoggedIn) UserName = App.CurrentAccount.Login;

        App.CurrentAccount.PropertyChanged += AccountStateUpdated;
    }


    void AccountStateUpdated(object sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(Account.IsLoggedIn)) return;
        IsLoggedIn = App.CurrentAccount.IsLoggedIn;
        UserName = App.CurrentAccount.Login;
    }

    [RelayCommand]
    async void SignIn()
    {
        await Shell.Current.GoToAsync($"{nameof(LoginPage)}");
    }
}
