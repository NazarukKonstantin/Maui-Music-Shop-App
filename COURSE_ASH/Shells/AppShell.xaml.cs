namespace COURSE_ASH;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();


        Routing.RegisterRoute(nameof(ImageUploadPage), typeof(ImageUploadPage));
        Routing.RegisterRoute(nameof(CartPage), typeof(CartPage));
        Routing.RegisterRoute(nameof(ProfilePage), typeof(ProfilePage));
        Routing.RegisterRoute(nameof(SearchPage), typeof(SearchPage));
        Routing.RegisterRoute(nameof(ProductPage), typeof(ProductPage));
        Routing.RegisterRoute(nameof(LoginPage), typeof(LoginPage));
        Routing.RegisterRoute(nameof(RegistrationPage), typeof(RegistrationPage));



    }
}
