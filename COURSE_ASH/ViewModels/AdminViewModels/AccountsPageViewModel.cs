namespace COURSE_ASH.ViewModels.AdminViewModels;

public partial class AccountsPageViewModel : BaseViewModel, IRefreshable
{
    private readonly AccountManager _service;

    [ObservableProperty]
    ObservableCollection<AccountData> _accounts;

    public AccountsPageViewModel(AccountManager accountsService)
    {
        _service = accountsService;
        RefreshAsync();
    }

    [RelayCommand]
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
            //await Shell.Current.DisplayAlert("ERROR", "Could not get users!", "OK");
            await Toast.Make(GeneralAlerts.NO_CONNECTION,ToastDuration.Short).Show();
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
            if (IsAccountImmutable(account))
            {
                await Shell.Current.DisplayAlert("ERROR!", $"Cannot switch role of {account.CurrentLogin} " +
                    "for safety matter", "OK");
                return;
            }
            IsBusy = true;
            account.Role = _service.SwitchRole(account.Role);
            await _service.RecordNewRoleAsync(account.CurrentLogin, account.Role);
            RefreshAsync();
        }
        catch (Exception)
        {
            //await Shell.Current.DisplayAlert("ERROR", "Could not record new role!", "OK");
            await Toast.Make(GeneralAlerts.NO_CONNECTION, ToastDuration.Short).Show();
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
            if (IsAccountImmutable(account))
            {
                await Shell.Current.DisplayAlert("ERROR!",$"Cannot delete {account.CurrentLogin} " +
                    "for safety matter","OK");
                return;
            }
            IsBusy = true;
            bool choice = await Shell.Current.DisplayAlert("Are you sure?!",
                $"Account {account.CurrentLogin} will be deleted",
                "Confirm",
                "Cancel");
            if (choice)
            {
                await _service.DeleteAccount(account.CurrentLogin);
                RefreshAsync();
            }
        }
        catch (Exception)
        {
            //await Shell.Current.DisplayAlert("ERROR", "Could not delete account!", "OK");
            await Toast.Make(GeneralAlerts.NO_CONNECTION, ToastDuration.Short).Show();
        }
        finally
        {
            IsBusy = false;
        }
    }


    public bool IsAccountImmutable(AccountData account)
    {
        return account.CurrentLogin.Equals(App.CurrentLogin)
            ||account.Role==Roles.Boss;
    }
}
