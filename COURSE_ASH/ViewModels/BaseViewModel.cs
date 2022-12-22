namespace COURSE_ASH.ViewModels;

public partial class BaseViewModel : ObservableObject
{
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsNotBusy))]
    bool _isBusy;
    public bool IsNotBusy => !IsBusy;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsNotSuccessful))]
    bool _isSuccessful = false;
    public bool IsNotSuccessful => !IsSuccessful;

    [ObservableProperty]
    private bool _isFailed = false;

    [ObservableProperty]
    private bool _isRefreshing = false;
}
