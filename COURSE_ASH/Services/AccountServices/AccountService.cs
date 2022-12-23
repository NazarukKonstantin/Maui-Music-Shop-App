namespace COURSE_ASH.Services.AccountServices;

public class AccountService
{
    protected static bool DoPasswordsMatch(string passwordOne, string passwordTwo)
    {
        return passwordOne == SHA256HashComputer.ComputeSha256Hash(passwordTwo);
    }

    protected static string SetPasswordSHA256(string newPassword)
    {
        return SHA256HashComputer.ComputeSha256Hash(newPassword);
    }
}
