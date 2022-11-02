using CommunityToolkit.Mvvm.Input;

using COURSE_ASH.View;

namespace COURSE_ASH.ViewModel
{
    public partial class ProfilePageViewModel : BaseViewModel
    {
        Image profilePic = new()
        {
            Source="add_user.svg"
        };

        [RelayCommand]
        async Task GoToProfileAsync()
        {
            await Shell.Current.GoToAsync($"{nameof(ProfilePage)}?Image={profilePic}", true);
        }
    }
}
