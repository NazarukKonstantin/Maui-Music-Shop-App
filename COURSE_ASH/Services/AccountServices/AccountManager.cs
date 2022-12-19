namespace COURSE_ASH.Services.AccountServices;

public class AccountManager
{
    public async Task<IEnumerable<AccountData>> GetUsersAsync()
    {
        return (from account in await DataStorageService<AccountData>.GetItemListAsync()
                select new AccountData
                {
                    Role = account.Role,
                    ID = account.ID,
                    CurrentLogin = account.CurrentLogin,
                });
    }
    public async Task RecordNewRoleAsync(string login, Roles newRole)
    {
        AccountData account = await DataStorageService<AccountData>.GetItemByAsync(nameof(AccountData.CurrentLogin), login);
        account.Role = newRole;
        await DataStorageService<AccountData>.UpdateItemAsync(account, nameof(AccountData.CurrentLogin), login);
    }

    public Roles SwitchRole(Roles role)
    {
        if (role is Roles.Boss) return role;
        return role == Roles.User ? Roles.Admin : Roles.User;
    }
    public async Task DeleteAccount(string login)
    {
        await DataStorageService<AccountData>.DeleteItemAsync(nameof(AccountData.CurrentLogin), login);
    }
}
