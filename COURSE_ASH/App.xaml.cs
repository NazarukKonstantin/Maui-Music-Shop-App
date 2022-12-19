using COURSE_ASH.Services;
using COURSE_ASH.Services.DataStorageStrategies;

namespace COURSE_ASH;

public partial class App : Application
{
    //Логин вошедшего пользователя для дальнейшей работы с аккаунтом и связанной информацией
    public static string CurrentLogin { get; set; }
    //Паттерн Стратегия - данный класс отвечает за создание хранилища информации
    //Тип хранилища может изменяться в зависимости от передаваемых стратегий
    //Объект FbClientManager отвечает за создание клиента Firebase Realtime Database по имеющейся URL
    public static DataStorageLocationManager<FirebaseClient> FbClientManager { get; } = new(FB_REALTIME_DB_URL);
    //Объект FbStorageManager отвечает за создание клиента Firebase Storage по имеющейся URL
    public static DataStorageLocationManager<FirebaseStorage> FbStorageManager { get; } = new(FB_STORAGE_URL);
    //Константные URL подключения к Google Firebase для изоляции URL от остального кода
    private const string FB_REALTIME_DB_URL = "https://musicshop-725ec-default-rtdb.europe-west1.firebasedatabase.app/";
    private const string FB_STORAGE_URL = "musicshop-725ec.appspot.com";

    public App()
    {
        //Создание основной разметки
        InitializeComponent();
        //Инициализация объекта главной страницы приложения
        MainPage = new AuthorizationShell();

        //Задание текущей стратегии использования БД
        FbClientManager.SetStorage(new FirebaseRealtimeDB());
        FbStorageManager.SetStorage(new FirebaseDataStorage());
    }
}
