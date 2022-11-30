namespace COURSE_ASH.Services;

public class AccountService
{
    public async Task<AccountConfirmator> CreateAccountAsync(string login, string password, string repeatPassword)
    {
        return await TempServer.CreateAccountAsync(login, password, repeatPassword);
    }
}
