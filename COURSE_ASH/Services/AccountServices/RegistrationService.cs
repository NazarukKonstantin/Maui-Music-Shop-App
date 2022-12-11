namespace COURSE_ASH.Services.AccountServices;

public class RegistrationService : AccountService
{
    public async Task<AccountState> RegisterUserAsync(string login, string password, string repeatPassword)
    {
        string state = AccountSafetyChecker.CheckRegistration(login, password, repeatPassword);
        if (state != AccountAlerts.SUCCESS) 
            return new AccountState(state);

        IEnumerable<AccountData> accounts =
            await DataStorageService<AccountData>.GetItemListAsync();

        if (DoLoginsMatch(accounts,login))
            return new AccountState(AccountAlerts.SAME_LOGIN_EXIST);

        string role = accounts.Any() ? Roles.User : Roles.Admin; // if no users -> the first one is admin

        await DataStorageService<AccountData>.AddItemAsync(new AccountData
        {
            ID=accounts.Count()+1,
            UserName=login,
            Password=SetPassword(password),
            Role=role,
        });

        return new AccountState(login, role, AccountAlerts.SUCCESS);
    }
}
