using static COURSE_ASH.Globals.GlobalConsts;
namespace COURSE_ASH.Globals;

public static class AccountAlerts
{
    public static readonly string PASSWORDS_DONT_MATCH = "Passwords are not the same!";
    public static readonly string INCORRECT_LOGIN_OR_PASSWORD = "Incorrect login or password!";
    public static readonly string CONNECTION_ERROR = "No connection!";
    public static readonly string FIELDS_EMPTY = "Some fields are empty!";
    public static readonly string SAME_LOGIN_EXIST = "Same login already exist!";
    public static readonly string SUCCESS = "Success!";
    public static readonly string LOGIN_OUT_OF_RANGE = $"Login must contain {MIN_LOGIN_LENGTH}-{MAX_LOGIN_LENGTH} symbols!";
    public static readonly string PASSWORD_OUT_OF_RANGE = $"Password must contain {MIN_PASSWORD_LENGTH}-{MAX_PASSWORD_LENGTH} symbols!";
    public static readonly string PASSWORD_IS_WEAK = "Your password is weak!";
    public static readonly string LOGIN_IS_INFORMAT = "Login can only contain a-z letters, \'-\' and/or \'_\' !";
    public static readonly string PASSWORD_SHOULD_CONTAIN = "It should contain at least one uppercase letter, one lowercase letter, one number and one special symbol";
    public static readonly string INCORRECT_OLD_PASSWORD = "Incorrect old password!";
}