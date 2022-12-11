namespace COURSE_ASH.Services.AccountServices;

public class LogInService : AccountService
{
    public async Task<AccountState> LogInUserAsync(string login, string password)
    {
        string state = AccountSafetyChecker.CheckAuthorization(login, password);
        if (state != AccountAlerts.SUCCESS) 
            return new AccountState(state);

        AccountData userAccount = await DataStorageService<AccountData>.GetItemAsync(nameof(AccountData.CurrentLogin),login);

        if (userAccount is null || !DoPasswordsMatch(userAccount.Password, password))
            return new AccountState(AccountAlerts.INCORRECT_LOGIN_OR_PASSWORD);

        return new AccountState(login, userAccount.Role, AccountAlerts.SUCCESS);
    }
}
