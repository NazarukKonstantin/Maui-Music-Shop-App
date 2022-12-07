namespace COURSE_ASH.Services.AccountServices;

public class LogInService
{
    private const string _dbURI = "https://electronicsshop-8c6b3-default-rtdb.europe-west1.firebasedatabase.app/";
    private readonly FirebaseClient _firebaseClient = new FirebaseClient(_dbURI);

    public async Task<AccountData> LogInUserAsync(string login, string password)
    {
        string state = AccountSafetyChecker.CheckAuthorization(login, password);
        if (state != AccountAlerts.SUCCESS) return new AccountData(null, null, state);

        FirebaseObject<DBAccountData> userAccount = await _firebaseClient
            .Child(nameof(DBAccountData))
            .OrderBy(nameof(DBAccountData.UserName))
            .EqualTo(login)
            .OnceSingleAsync<FirebaseObject<DBAccountData>>();

        if (userAccount is null || userAccount.Object.Password != SHA256HashComputer.ComputeSha256Hash(password))
            return new AccountData(null, null, AccountAlerts.INCORRECT_LOGIN_OR_PASSWORD);

        return new AccountData(login, userAccount.Object.Role, AccountAlerts.SUCCESS);
    }

    //
    public async Task<AccountData> LogInAsync(string login, string password)
    {
        return await TempServer.LogInAsync(login, password);
    }
}
