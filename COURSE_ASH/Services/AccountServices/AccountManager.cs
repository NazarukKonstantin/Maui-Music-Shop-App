namespace COURSE_ASH.Services.AccountServices;

public class AccountManager
{
    public async Task<IEnumerable<AccountData>> GetUsersAsync()
    {
        return (from account in await DataStorageService<AccountData>.GetItemListAsync()
                select new AccountData
                {
                    Role = account.Role,
                    IsAdmin = account.IsAdmin,
                    ID = account.ID,
                    CurrentLogin = account.CurrentLogin,
                });
    }
    public async Task RecordNewRoleAsync(string login, string newRole)
    {
        AccountData account = await DataStorageService<AccountData>.GetItemByAsync(nameof(AccountData.CurrentLogin), login);
        account.Role = newRole;
        DefineRoleState(account);
        await DataStorageService<AccountData>.UpdateItemAsync(account, nameof(AccountData.CurrentLogin), login);
    }

    public string SwitchRole(string role)
    {
        if (role is Roles.Boss) return role;
        return role == Roles.User ? Roles.Admin : Roles.User;
    }
    public async Task DeleteAccount(string login)
    {
        await DataStorageService<AccountData>.DeleteItemsAsync(nameof(AccountData.CurrentLogin), login);
    }

    public static void DefineRoleState(AccountData account)
    {
        account.IsBoss = account.Role == Roles.Boss;
        account.IsAdmin = (account.Role == Roles.Admin || account.IsBoss);
    }
}
