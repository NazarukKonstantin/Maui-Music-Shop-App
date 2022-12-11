namespace COURSE_ASH.ViewModels.UserViewModels;

[QueryProperty(nameof(CurrentLogin),nameof(CurrentLogin))]
public partial class OrderHistoryPageViewModel : BaseViewModel
{
    [ObservableProperty]
    private string _currentLogin;
}
