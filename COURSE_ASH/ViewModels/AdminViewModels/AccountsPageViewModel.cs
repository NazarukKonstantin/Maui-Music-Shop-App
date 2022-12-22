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
        try
        {
            IsRefreshing=true;
            IsBusy=true;
            Accounts = (await _service
                .GetUsersAsync())
                .ToObservableCollection();
        }
        catch (Exception)
        {
            //await _popup.NotifyAsync("Could not get users!");
        }
        finally
        {
            IsRefreshing = false;
            IsBusy=false;
        }
    }

    [RelayCommand]
    private async Task SwitchRoleAsync(AccountData account)
    {
        try
        {
            if (IsAccountImmutable(account)) return;
            IsBusy = true;
            account.Role = _service.SwitchRole(account.Role);
            await _service.RecordNewRoleAsync(account.CurrentLogin, account.Role);
            IsBusy = false;
        }
        catch (Exception)
        {
            //await _popup.NotifyAsync("Could not record new role!");
        }
        finally
        {
            IsBusy=false;
        }
    }

    [RelayCommand]
    async Task DeleteAccountAsync(AccountData account)
    {
        try
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
        }
        catch (Exception)
        {
            //await _popup.NotifyAsync("Could not delete account!");
        }
        finally
        {
            IsBusy = false;
        }
    }

    private bool IsAccountImmutable(AccountData account)
    {
        return account.CurrentLogin.Equals(App.CurrentLogin)
            ||account.Role==Roles.Boss;
    }
}
