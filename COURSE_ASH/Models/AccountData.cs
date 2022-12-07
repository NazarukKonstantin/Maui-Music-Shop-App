namespace COURSE_ASH.Models;

public class AccountData
{
    public string Login { get; set; }
    public string Role { get; set; }
    public string Alert { get; set; }

    public AccountData(string login, string role,string alert)
    {
        Login=login;
        Role=role;
        Alert=alert;
    }
}
