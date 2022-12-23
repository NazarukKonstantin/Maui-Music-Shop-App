namespace COURSE_ASH.Models;

public class AccountState
{
    public string Login { get; set; }
    public string Role { get; set; }
    public string Alert { get; set; }

    public AccountState(string login, string role,string alert)
    {
        Login=login;
        Role=role;
        Alert=alert;
    }

    public AccountState(string alert)
    {
        Login=string.Empty;
        Role=default;
        Alert=alert;
    }
}
