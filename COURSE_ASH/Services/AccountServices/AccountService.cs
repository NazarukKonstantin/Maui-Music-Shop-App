namespace COURSE_ASH.Services.AccountServices;

public class AccountService
{
    protected static bool DoPasswordsMatch(string passwordOne, string passwordTwo)
    {
        return passwordOne == SHA256HashComputer.ComputeSha256Hash(passwordTwo);
    }

    protected static bool DoLoginsMatch(IEnumerable<AccountData> accounts, string login)
    {
        return (from acc in accounts where acc.CurrentLogin == login select acc).Any();
    }

    protected static string SetPasswordSHA256(string newPassword)
    {
        return SHA256HashComputer.ComputeSha256Hash(newPassword);
    }
}
