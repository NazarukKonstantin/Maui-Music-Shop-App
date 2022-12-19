namespace COURSE_ASH.ViewModels.AuthorizationViewModels;

public partial class LogInPageViewModel : BaseViewModel
{
    //Объект сервиса по работе с авторизацией и чтением аккаунта из БД
    readonly LogInService _logInService;

    //Атрибут, позволяющий удобно реализовать паттерн Наблюдатель
    [ObservableProperty]
    //Поле, хранящее значение введённого пользователем логина
    private string _login;

    [ObservableProperty]
    //Поле, хранящее значение введённого пользователем пароля
    private string _password;

    [ObservableProperty]
    //Поле, хранящее значение состояния видимости пароля при вводе
    private bool _isPasswordVisible;

    [ObservableProperty]
    //Поле, хранящее значение возможной ошибки при регистрации
    private string _alert;

    //Аргументом конструктора выступает объект сервиса авторизации
    public LogInPageViewModel(LogInService logInService)
    {
        _logInService = logInService;
    }

    //Атрибут, позволяющий удобно реализовать паттерн Команда
    [RelayCommand]
    //Метод отвечает за вход в аккаунт
    public async Task LogIn()
    {
        IsBusy = true;
        //Проверка введённых данных производится сервисом.
        //Вызванный метод возвращает текущий логин, роль пользователя и возможную ошибку при авторизации
        AccountState accountState = await _logInService.LogInUserAsync(Login, Password);
        //Если отсутствуют ошибки при авторизации, открыть соответствующую роли главную страницу
        if (accountState.Alert == AccountAlerts.SUCCESS)
        {
            IsSuccessful = true;
            IsFailed = false;
            App.CurrentLogin = accountState.Login;
            if (accountState.Role == Roles.Admin
                || accountState.Role==Roles.Boss)
                App.Current.MainPage = new AdminShell();
            else
                App.Current.MainPage = new AppShell();
        }
        //Иначе присвоить полям IsSuccessful и Alert соответствующие значения
        else
        {
            IsSuccessful = false;
            Alert = accountState.Alert;
            IsFailed = true;
            Password = string.Empty;
        }
        IsBusy = false;
    }

    [RelayCommand]
    //Метод отвечает за переход на страницу регистрации
    async Task SignUp()
    {
        IsBusy = true;
        Login = string.Empty;
        Password = string.Empty;
        IsFailed = false;
        IsSuccessful = false;
        await Shell.Current.GoToAsync(nameof(RegistrationPage));
        IsBusy = false;
    }
}