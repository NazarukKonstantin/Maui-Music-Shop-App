namespace COURSE_ASH.Services.AccountServices;

public class PasswordChangingService : AccountService
{
    public async Task<AccountState> ChangePasswordAsync(string login, string oldPassword, string newPassword, string repeatPassword)
    {
        string state = AccountSafetyChecker.CheckPasswordChange(oldPassword, newPassword, repeatPassword, out bool isValid);
        if (!isValid)
        {
            bool choice = await Shell.Current.DisplayAlert("Are you sure?", $"{state}", "Confirm", "Cancel");
            if (!choice)
            {
                return new AccountState(state);
            }
        }
        else if (isValid && state != AccountAlerts.SUCCESS)
            return new AccountState(state);

        AccountData account = await DataStorageService<AccountData>.GetItemByAsync(nameof(AccountData.CurrentLogin), login);

        if (account is null || !DoPasswordsMatch(account.Password,oldPassword))
            return new AccountState(AccountAlerts.INCORRECT_OLD_PASSWORD);

        account.Password = SetPasswordSHA256(newPassword);

        await DataStorageService<AccountData>.UpdateItemAsync(account,nameof(AccountData.CurrentLogin),login); 

        return new AccountState(login, account.Role, AccountAlerts.SUCCESS);
    }
}
