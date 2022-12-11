namespace COURSE_ASH.Shells;

public partial class AuthorizationShell : Shell
{
	public AuthorizationShell()
	{
		InitializeComponent();

		Routing.RegisterRoute(nameof(LoginPage), typeof(LoginPage));
        Routing.RegisterRoute(nameof(RegistrationPage), typeof(RegistrationPage));
    }
}