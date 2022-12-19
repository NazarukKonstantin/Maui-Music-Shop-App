namespace COURSE_ASH.ViewModels.AdminViewModels;

public partial class AccountsPageViewModel : BaseViewModel
{
    private readonly AccountManager _service;

    [ObservableProperty]
    ObservableCollection<AccountData> _accounts;

    //Clear
    [ObservableProperty]
    private Roles _role;


    public AccountsPageViewModel(AccountManager accountsService)
    {
        _service = accountsService;
        RefreshAsync();
    }
    public async void RefreshAsync()
    {
        Accounts = (await _service
            .GetUsersAsync())
            .ToObservableCollection();
    }

    [RelayCommand]
    private async Task SwitchRoleAsync(AccountData account)
    {
        if (IsAccountImmutable(account)) return;
        IsBusy = true;
        account.Role = _service.SwitchRole(account.Role);
        await _service.RecordNewRoleAsync(account.CurrentLogin, account.Role);
        IsBusy = false;
    }

    [RelayCommand]
    async Task DeleteAccountAsync(AccountData account)
    {
        if (IsAccountImmutable(account)) return;
        IsBusy = true;
        bool choice = await Shell.Current.DisplayAlert("Are you sure?!",
            $"Account {account.CurrentLogin} will be deleted",
            "Confirm",
            "Cancel");
        if (choice)
        {
            await _service.DeleteAccount(account.CurrentLogin);
        }
        IsBusy = false;
    }

    [RelayCommand]
    private async Task GoBackAsync()
    {
        await Shell.Current.GoToAsync("..");
    }

    private bool IsAccountImmutable(AccountData account)
    {
        return account.CurrentLogin.Equals(App.CurrentLogin)
            ||account.Role==Roles.Boss;
    }
}
