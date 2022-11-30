namespace COURSE_ASH.Services;

public class LogInService
{
    public async Task<AccountConfirmator> LogInAsync(string login, string password)
    {
        return await TempServer.LogInAsync(login, password);
    }
}
