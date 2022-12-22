namespace COURSE_ASH.ViewModels.AuthorizationViewModels;

public partial class RegistrationPageViewModel: BaseViewModel
{
    //Объект сервиса по работе с регистрацией и записью аккаунта в БД
    private readonly RegistrationService _service;

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
    //Поле, хранящее значение введённого пользователем подтверждения пароля
    private string _confirmPassword;

    [ObservableProperty]
    //Поле, хранящее значение возможной ошибки при регистрации
    private string _alert;

    //Аргументом конструктора выступает объект сервиса регистрации
    public RegistrationPageViewModel(RegistrationService service)
    {
        _service=service;
    }

    //Атрибут, позволяющий удобно реализовать паттерн Команда
    [RelayCommand]
    //Метод, отвечающий за создание аккаунта
    public async Task CreateAccountAsync()
    {
        try
        {
            IsBusy = true;
            //Проверка введённых полей производится сервисом.
            //Вызванный метод возвращает текущий логин, роль пользователя и возможную ошибку при регистрации 
            AccountState accountState = await _service.RegisterAsync(Login, Password, ConfirmPassword);
            //Если присутствуют ошибки при регистрации, присвоить полям IsSuccessful и Alert соответствующие значения
            if (accountState.Alert != AccountAlerts.SUCCESS)
            {
                IsSuccessful = false;
                Alert = accountState.Alert;
            }
            else IsSuccessful = true;
        }
        catch (Exception)
        {
            await Shell.Current.DisplayAlert("ERROR", "Could not create account!", "OK");
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    //Метод отвечает за возврат на предыдущую страницу
    async Task GoBackAsync()
    {
        await Shell.Current.GoToAsync("..");
    }
}
