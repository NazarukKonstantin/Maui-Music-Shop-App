namespace COURSE_ASH.Services.AccountServices;

public class RegistrationService
{
    private const string _dbURI = "https://electronicsshop-8c6b3-default-rtdb.europe-west1.firebasedatabase.app/";
    private readonly FirebaseClient _firebaseClient = new (_dbURI);

    public async Task<AccountData> RegisterUserAsync(string login, string password, string repeatPassword)
    {
        string state = AccountSafetyChecker.CheckRegData(login, password, repeatPassword);
        if (state != AccountAlerts.SUCCESS) return new AccountData(null, null, state);

        IReadOnlyCollection<FirebaseObject<DBAccountData>> accounts = await _firebaseClient
            .Child(nameof(DBAccountData))
            .OnceAsync<DBAccountData>();
        if ((from acc in accounts where acc.Object.UserName == login select acc).Any())
            return new AccountData(null, null, AccountAlerts.SAME_LOGIN_EXIST);
        string role = accounts.Any() ? Roles.User : Roles.Admin;
        await _firebaseClient.Child(nameof(DBAccountData)).PostAsync(new DBAccountData
        {
            ID = accounts.Count + 1,
            UserName = login,
            Password = SHA256HashComputer.ComputeSha256Hash(password),
            Role = role
        });

        return new AccountData(login, role, AccountAlerts.SUCCESS);
    }
}
