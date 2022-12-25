using System.Text.RegularExpressions;
using static COURSE_ASH.Globals.AccountAlerts;
using static COURSE_ASH.Globals.GlobalConsts;
namespace COURSE_ASH.Services.AccountServices;

public static class AccountSafetyChecker
{
    private enum Field
    {
        LOGIN, PASSWORD
    };

    private static readonly string _loginRegex = @"^[a-zA-Z0-9_-]{3,15}$";
    private static readonly string _passwordRegex = @"^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$ %^&*-]).{8,}$";


    public static string CheckRegistration (string login, string password, string confirmPassword, out bool isValid)
    {
        isValid = true;
        if (IsFieldEmpty(login, password, confirmPassword))
        {
            return FIELDS_EMPTY;
        }

        if (!IsLengthInRangeIn(Field.LOGIN, login))
        {
            return LOGIN_OUT_OF_RANGE;
        }

        if (!IsLengthInRangeIn(Field.PASSWORD, password))
        {
            return PASSWORD_OUT_OF_RANGE;
        }

        if (password != confirmPassword)
        {
            return PASSWORDS_DONT_MATCH;
        }

        if (!Regex.IsMatch(login, _loginRegex, RegexOptions.IgnoreCase))
        {
            return LOGIN_IS_INFORMAT;
        }


        if (!Regex.IsMatch(password, _passwordRegex))
        {
            isValid = false;
            return PASSWORD_IS_WEAK;
        }

        return SUCCESS;
    }

    public static string CheckAuthorization (string login, string password)
    {
        if (IsFieldEmpty(login, password))
            return FIELDS_EMPTY;

        if (!IsLengthInRangeIn(Field.LOGIN, login))
            return LOGIN_OUT_OF_RANGE;

        if (!IsLengthInRangeIn(Field.PASSWORD, password))
            return PASSWORD_OUT_OF_RANGE;

        return SUCCESS;
    }

    public static string CheckPasswordChange (string oldPassword, string newPassword, string repeatPassword, out bool isValid)
    {
        isValid = true;

        if (IsFieldEmpty(oldPassword, newPassword, repeatPassword))
            return FIELDS_EMPTY;

        if (!IsLengthInRangeIn(Field.PASSWORD,oldPassword) 
            || !IsLengthInRangeIn(Field.PASSWORD, newPassword)
            || !IsLengthInRangeIn(Field.PASSWORD,repeatPassword))
            return PASSWORD_OUT_OF_RANGE;

        if (newPassword != repeatPassword)
            return PASSWORDS_DONT_MATCH;

        if (!Regex.IsMatch(newPassword, _passwordRegex))
        {
            isValid = false;
            return PASSWORD_IS_WEAK;
        }

        return SUCCESS;
    }

    private static bool IsLengthInRangeIn(Field fieldName, string entryText)
    {
        return fieldName switch
        {
            Field.LOGIN => (entryText.Length >= MIN_LOGIN_LENGTH && entryText.Length<=MAX_LOGIN_LENGTH),
            Field.PASSWORD => (entryText.Length >= MIN_PASSWORD_LENGTH && entryText.Length<=MAX_PASSWORD_LENGTH),
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
