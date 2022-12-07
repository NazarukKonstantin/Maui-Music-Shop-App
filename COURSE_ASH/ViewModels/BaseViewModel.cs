namespace COURSE_ASH.ViewModels;

public partial class BaseViewModel : ObservableObject
{
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsNotBusy))]
    bool _isBusy = false;
    public bool IsNotBusy => !_isBusy;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsNotSuccessful))]
    bool isSuccessful = false;
    public bool IsNotSuccessful => !isSuccessful;

    [ObservableProperty]
    public bool isFailed = false;
}
