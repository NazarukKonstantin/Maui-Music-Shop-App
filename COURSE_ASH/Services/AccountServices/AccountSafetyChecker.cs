using static COURSE_ASH.Globals.AccountAlerts;
namespace COURSE_ASH.Services.AccountServices;

public static class AccountSafetyChecker
{
    private const int MIN_LOGIN_LENGTH = 8;
    private const int MIN_PASSWORD_LENGTH = 8;

    private enum Field
    {
        LOGIN, PASSWORD
    };

    public static string CheckRegistration (string login, string password, string confirmPassword)
    {
        if (IsFieldEmpty(login, password, confirmPassword))
            return FIELDS_EMPTY;

        if (!IsLengthEnoughIn(Field.LOGIN,login) || !IsLengthEnoughIn(Field.PASSWORD, password))
            return LOGIN_OR_PASSWORD_TOO_SHORT;

        if (password != confirmPassword)
            return PASSWORDS_DONT_MATCH;

        return SUCCESS;
    }

    public static string CheckAuthorization (string login, string password)
    {
        if (IsFieldEmpty(login, password))
            return FIELDS_EMPTY;

        if (!IsLengthEnoughIn(Field.LOGIN, login) || !IsLengthEnoughIn(Field.PASSWORD, password))
            return LOGIN_OR_PASSWORD_TOO_SHORT;

        return SUCCESS;
    }

    public static string CheckPasswordChange (string oldPassword, string newPassword, string repeatPassword)
    {
        if (IsFieldEmpty(oldPassword, newPassword, repeatPassword))
            return FIELDS_EMPTY;

        if (!IsLengthEnoughIn(Field.PASSWORD,oldPassword) || !IsLengthEnoughIn(Field.PASSWORD, newPassword)|| !IsLengthEnoughIn(Field.PASSWORD,repeatPassword))
            return PASSWORD_TOO_SHORT;

        if (newPassword != repeatPassword)
            return PASSWORDS_DONT_MATCH;

        return SUCCESS;
    }

    private static bool IsLengthEnoughIn(Field fieldName, string entryText)
    {
        return fieldName switch
        {
            Field.LOGIN => entryText.Length >= MIN_LOGIN_LENGTH,
            Field.PASSWORD => entryText.Length >= MIN_PASSWORD_LENGTH,
            _=>false,
        };
    }

    private static bool IsFieldEmpty(params string[] entryList)
    {
        foreach (var entry in entryList)
            if (string.IsNullOrEmpty(entry))
                return true;
        return false;
    }
}
