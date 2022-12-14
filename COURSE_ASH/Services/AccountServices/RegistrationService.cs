namespace COURSE_ASH.Services.AccountServices;

public class RegistrationService : AccountService
{
    public async Task<AccountState> RegisterAsync(string login, string password, string repeatPassword)
    {
        string state = AccountSafetyChecker.CheckRegistration(login, password, repeatPassword, out bool isValid);
        if (!isValid)
        {
            bool choice = await Shell.Current.DisplayAlert("Confirm input data?", $"{state}", "Confirm", "Cancel");
            if (!choice)
            {
                return new AccountState(state);
            }
        }
        else if (isValid && state != AccountAlerts.SUCCESS)
            return new AccountState(state);

        IEnumerable<AccountData> accounts =
            await DataStorageService<AccountData>.GetItemListAsync();

        if ((from acc in accounts where acc.CurrentLogin == login select acc).Any())
            return new AccountState(AccountAlerts.SAME_LOGIN_EXIST);

        string role = accounts.Any() ? Roles.User : Roles.Boss; // if no users -> the first one is boss

        int newID = (from account
                       in accounts
                     select account.ID)?.Max() + 1 ?? 1;

        var newAcc = new AccountData
        {
            ID = newID,
            CurrentLogin = login,
            Password = SetPasswordSHA256(password),
            Role = role,
        };
        AccountManager.DefineRoleState(newAcc);
        await DataStorageService<AccountData>.AddItemAsync(newAcc);

        return new AccountState(login, role, AccountAlerts.SUCCESS);
    }
}