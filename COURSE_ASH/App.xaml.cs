using COURSE_ASH.Services;
using COURSE_ASH.Services.DataStorageStrategies;

namespace COURSE_ASH;

public partial class App : Application
{
    public static string CurrentLogin { get; set; }
    public static DataStorageLocationManager<FirebaseClient> FbClientManager { get; } = new(FB_REALTIME_DB_URL);
    public static DataStorageLocationManager<FirebaseStorage> FbStorageManager { get; } = new(FB_STORAGE_URL);
    private const string FB_REALTIME_DB_URL = "https://musicshop-725ec-default-rtdb.europe-west1.firebasedatabase.app/";
    private const string FB_STORAGE_URL = "musicshop-725ec.appspot.com";

    public App()
    {
        InitializeComponent();
        MainPage = new AuthorizationShell();

        FbClientManager.SetDataStorage(new FirebaseRealtimeDB());
        FbStorageManager.SetDataStorage(new FirebaseDataStorage());
    }
}
