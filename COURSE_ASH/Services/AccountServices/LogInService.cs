namespace COURSE_ASH.Services.AccountServices;

public class LogInService : AccountService
{
    public async Task<AccountState> LogInUserAsync(string login, string password)
    {
        string state = AccountSafetyChecker.CheckAuthorization(login, password);
        if (state != AccountAlerts.SUCCESS) 
            return new AccountState(state);

        AccountData account = await DataStorageService<AccountData>
            .GetItemByAsync(nameof(AccountData.CurrentLogin),login);

        if (account is null || !DoPasswordsMatch(account.PasswordSHA256, password))
            return new AccountState(AccountAlerts.INCORRECT_LOGIN_OR_PASSWORD);

        return new AccountState(login, account.Role, AccountAlerts.SUCCESS);
    }
}
