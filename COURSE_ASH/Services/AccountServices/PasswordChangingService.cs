namespace COURSE_ASH.Services.AccountServices;

public class PasswordChangingService
{
    private const string _dbURI = "https://electronicsshop-8c6b3-default-rtdb.europe-west1.firebasedatabase.app/";
    private readonly FirebaseClient _firebaseClient = new FirebaseClient(_dbURI);


    public async Task<AccountData> ChangePasswordAsync(string login, string oldPassword, string newPassword, string repeatPassword)
    {
        string state = AccountSafetyChecker.CheckPasswordChangeData(oldPassword, newPassword, repeatPassword);
        if (state != AccountAlerts.SUCCESS) return new AccountData(null, null, state);

        FirebaseObject<DBAccountData> userAccount = (await _firebaseClient
            .Child(nameof(DBAccountData))
            .OrderBy(nameof(DBAccountData.UserName))
            .EqualTo(login)
            .OnceAsync<DBAccountData>())
            .FirstOrDefault((FirebaseObject<DBAccountData>)null);

        if (userAccount.Object.Password != SHA256HashComputer.ComputeSha256Hash(oldPassword))
            return new AccountData(null, null, AccountAlerts.INCORRECT_LOGIN_OR_PASSWORD);
        userAccount.Object.Password = SHA256HashComputer.ComputeSha256Hash(newPassword);
        await _firebaseClient.Child(nameof(DBAccountData))
            .Child(userAccount.Key)
            .PatchAsync(userAccount.Object);

        return new AccountData(login, userAccount.Object.Role, AccountAlerts.SUCCESS);
    }
}
