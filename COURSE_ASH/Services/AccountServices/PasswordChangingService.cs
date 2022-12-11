namespace COURSE_ASH.Services.AccountServices;

public class PasswordChangingService : AccountService
{
    public async Task<AccountState> ChangePasswordAsync(string login, string oldPassword, string newPassword, string repeatPassword)
    {
        string state = AccountSafetyChecker.CheckPasswordChange(oldPassword, newPassword, repeatPassword);
        if (state != AccountAlerts.SUCCESS) return new AccountState(state);

        AccountData userAccount = await DataStorageService<AccountData>.GetItemAsync(nameof(AccountData.CurrentLogin), login);

        if (userAccount is null || !DoPasswordsMatch(userAccount.Password,oldPassword))
            return new AccountState(AccountAlerts.INCORRECT_OLD_PASSWORD);

        userAccount.Password = SetPassword(newPassword);

        await DataStorageService<AccountData>.UpdateItemAsync(userAccount,nameof(AccountData.CurrentLogin),login); 

        return new AccountState(login, userAccount.Role, AccountAlerts.SUCCESS);
    }
}
