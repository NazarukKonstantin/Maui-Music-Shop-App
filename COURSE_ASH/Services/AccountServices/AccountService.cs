namespace COURSE_ASH.Services.AccountServices;

public class AccountService
{
  
    //
    public async Task<AccountData> CreateAccountAsync(string login, string password, string repeatPassword)
    {
        return await TempServer.CreateAccountAsync(login, password, repeatPassword);
    }
}
