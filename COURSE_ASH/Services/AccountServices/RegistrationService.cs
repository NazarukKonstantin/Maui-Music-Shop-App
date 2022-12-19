namespace COURSE_ASH.Services.AccountServices;

public class RegistrationService : AccountService
{
    public async Task<AccountState> RegisterAsync(string login, string password, string repeatPassword)
    {
        string state = AccountSafetyChecker.CheckRegistration(login, password, repeatPassword);
        if (state != AccountAlerts.SUCCESS) 
            return new AccountState(state);

        IEnumerable<AccountData> accounts =
            await DataStorageService<AccountData>.GetItemListAsync();

        if (DoLoginsMatch(accounts,login))
            return new AccountState(AccountAlerts.SAME_LOGIN_EXIST);

        Roles role = accounts.Any() ? Roles.User : Roles.Boss; // if no users -> the first one is boss
        int newID = (from account
                     in accounts 
                     select account.ID)?.Max() + 1 ?? 1;

        await DataStorageService<AccountData>.AddItemAsync(new AccountData
        {
            ID=newID,
            CurrentLogin=login,
            PasswordSHA256=SetPasswordSHA256(password),
            Role=role,
        });

        return new AccountState(login, role, AccountAlerts.SUCCESS);
    }
}