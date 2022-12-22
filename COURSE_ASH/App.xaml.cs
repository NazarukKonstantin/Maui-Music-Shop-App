namespace COURSE_ASH;

public partial class App : Application
{
    //Логин вошедшего пользователя для дальнейшей работы с аккаунтом и связанной информацией
    public static string CurrentLogin { get; set; }
    public App()
    {
        //Создание основной разметки
        InitializeComponent();
        //Инициализация объекта главной страницы приложения
        MainPage = new AuthorizationShell();
    }
}
