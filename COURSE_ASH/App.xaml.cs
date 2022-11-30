namespace COURSE_ASH;

public partial class App : Application
{
    public static Account CurrentAccount { get; set; }
    public App()
    {
        InitializeComponent();
        MainPage = new AppShell();
    }
}
