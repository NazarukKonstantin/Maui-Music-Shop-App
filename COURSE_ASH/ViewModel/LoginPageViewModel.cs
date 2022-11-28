using CommunityToolkit.Mvvm.ComponentModel;
using COURSE_ASH.Model;

namespace COURSE_ASH.ViewModel
{
    public partial class LoginPageViewModel : BaseViewModel
    {
        [ObservableProperty]
        Account user;

        public LoginPageViewModel()
        {

        }
    }
}
