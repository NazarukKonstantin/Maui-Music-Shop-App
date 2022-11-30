namespace COURSE_ASH.Model;

public partial class Account : ObservableObject
{
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsNotLoggedIn))]
    bool _isLoggedIn;

    public bool IsNotLoggedIn => !_isLoggedIn;
    public string Login { get; set; }
    
    public Account()
    {
        Login = null;
        IsLoggedIn = false;
    }
}
