using COURSE_ASH.Services;

namespace COURSE_ASH;

public partial class App : Application
{
    public static string CurrentLogin { get; set; }
    public static DataBaseManager DBManager { get; private set; }

    public App()
    {
        InitializeComponent();
        MainPage = new AppShell();

        DBManager = new("https://musicshop-725ec-default-rtdb.europe-west1.firebasedatabase.app/");
    }
}
