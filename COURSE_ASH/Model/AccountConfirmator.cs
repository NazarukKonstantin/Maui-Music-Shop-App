namespace COURSE_ASH.Model;

public class AccountConfirmator
{
    public string Login { get; set; }
    public string Alert { get; set; }
    public AccountConfirmator(string login, string alert)
    {
        Login=login;
        Alert=alert;
    }
}
