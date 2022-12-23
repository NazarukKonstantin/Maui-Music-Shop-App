using CommunityToolkit.Maui;
using SimpleRatingControlMaui;
using SkiaSharp.Views.Maui.Controls.Hosting;

namespace COURSE_ASH;

public static class MauiProgram
{
    //Паттерн Строитель - пошаговое создание сложного объекта,
    //которым является данное приложение, состоящее из страниц
    public static MauiApp CreateMauiApp()
    {
        //Создание строителя
        var builder = MauiApp.CreateBuilder();
        //Добавление используемых библиотек
        builder
            .UseMauiApp<App>()
            .UseSkiaSharp()
            .UseSimpleRatingControl()
            .UseMauiCommunityToolkit()
            .UseMauiCommunityToolkitCore()
            //Добавление используемых шрифтов
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                fonts.AddFont("Rubik-Light.ttf", "RubikLight");
                fonts.AddFont("Rubik-Regular.ttf", "Rubik");
                fonts.AddFont("FiraSans-Bold.ttf", "FSBold");
                fonts.AddFont("FiraSans-BoldItalic.ttf", "FSBoldItalic");
                fonts.AddFont("FiraSans-Italic.ttf", "FSItalic");
                fonts.AddFont("FiraSans-Light.ttf", "FSLight");
                fonts.AddFont("FiraSans-LightItalic.ttf", "FSLightItalic");
                fonts.AddFont("FiraSans-Medium.ttf", "FSMedium");
                fonts.AddFont("FiraSans-MediumItalic.ttf", "FSMediumItalic");
                fonts.AddFont("FiraSans-Regular.ttf", "FSRegular");
                fonts.AddFont("Karla-Bold.ttf", "KarlaBold");
                fonts.AddFont("Karla-SemiBold.ttf", "KarlaSemiBold");
            });
        //Создание используемых View и ViewModel
        //Model не нуждается в создании в объекте приложения, так как создаётся
        //непосредственно во ViewModel

        builder.Services.AddTransient<AccountsPage>();
        builder.Services.AddTransient<AccountsPageViewModel>();

        builder.Services.AddTransient<AddCategoryPage>();
        builder.Services.AddTransient<AddCategoryPageViewModel>();

        builder.Services.AddTransient<AddProductPage>();
        builder.Services.AddTransient<AddProductPageViewModel>();

        builder.Services.AddTransient<AdminSearchPage>();
        builder.Services.AddTransient<AdminSearchPageViewModel>();

        builder.Services.AddTransient<EditCategoryPage>();
        builder.Services.AddTransient<EditCategoryPageViewModel>();

        builder.Services.AddTransient<EditProductPage>();
        builder.Services.AddTransient<EditProductPageViewModel>();

        builder.Services.AddTransient<OrdersCatalogPage>();
        builder.Services.AddTransient<OrdersCatalogPageViewModel>();




        builder.Services.AddTransient<LoginPage>();
        builder.Services.AddTransient<LogInPageViewModel>();

        builder.Services.AddTransient<RegistrationPage>();
        builder.Services.AddTransient<RegistrationPageViewModel>();
        



        builder.Services.AddTransient<CartPage>();
        builder.Services.AddTransient<CartPageViewModel>();

        builder.Services.AddTransient<CatalogPage>();
        builder.Services.AddTransient<CatalogPageViewModel>();

        builder.Services.AddTransient<CheckoutPage>();
        builder.Services.AddTransient<CheckoutPageViewModel>();

        builder.Services.AddTransient<FavouritesPage>();
        builder.Services.AddTransient<FavouritesPageViewModel>();

        builder.Services.AddTransient<OrderHistoryPage>();
        builder.Services.AddTransient<OrderHistoryPageViewModel>();

        builder.Services.AddTransient<OrderPage>();
        builder.Services.AddTransient<OrderHistoryPageViewModel>();

        builder.Services.AddTransient<ProductPage>();
        builder.Services.AddTransient<ProductPageViewModel>();

        builder.Services.AddTransient<ProfilePage>();
        builder.Services.AddTransient<ProfilePageViewModel>();

        builder.Services.AddTransient<SearchPage>();
        builder.Services.AddTransient<SearchPageViewModel>();

        //Паттерн Одиночка - объекты сервисов создаются
        //строителем в единственном экземпляре и к ним предоставляется
        //глобальная точка доступа
        builder.Services.AddSingleton<AccountManager>();
        builder.Services.AddSingleton<LogInService>();
        builder.Services.AddSingleton<PasswordChangingService>();
        builder.Services.AddSingleton<RegistrationService>();

        builder.Services.AddSingleton<BillingAddressService>();
        builder.Services.AddSingleton<ProductsService>();
        builder.Services.AddSingleton<CartService>();
        builder.Services.AddSingleton<OrderService>();
        builder.Services.AddSingleton<FavouritesService>();

        //Метод возвращает готовый объект приложения
        return builder.Build();
    }
}
