namespace COURSE_ASH;
using CommunityToolkit.Maui;
using COURSE_ASH.Views.AdminViews;
using SimpleRatingControlMaui;
using SkiaSharp.Views.Maui.Controls.Hosting;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .UseSkiaSharp()
            .UseSimpleRatingControl()
            .UseMauiCommunityToolkit()
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

        builder.Services.AddTransient<AddProductPage>();
        builder.Services.AddTransient<AddProductPageViewModel>();

        builder.Services.AddTransient<AdminSearchPage>();
        builder.Services.AddTransient<AdminSearchPageViewModel>();

        builder.Services.AddTransient<EditProductPage>();
        builder.Services.AddTransient<EditProductPageViewModel>();

        builder.Services.AddTransient<CheckoutPage>();
        builder.Services.AddTransient<CheckoutPageViewModel>();

        builder.Services.AddTransient<OrderHistoryPage>();
        builder.Services.AddTransient<OrderHistoryPageViewModel>();

        builder.Services.AddTransient<CatalogPage>();
        builder.Services.AddTransient<CatalogPageViewModel>();

        builder.Services.AddTransient<CartPage>();
        builder.Services.AddTransient<CartPageViewModel>();

        builder.Services.AddTransient<ProductPage>();
        builder.Services.AddTransient<ProductPageViewModel>();

        builder.Services.AddTransient<ProfilePage>();
        builder.Services.AddTransient<ProfilePageViewModel>();

        builder.Services.AddTransient<SearchPage>();
        builder.Services.AddTransient<SearchPageViewModel>();

        builder.Services.AddTransient<LoginPage>();
        builder.Services.AddTransient<LogInPageViewModel>();

        builder.Services.AddTransient<RegistrationPage>();
        builder.Services.AddTransient<RegistrationPageViewModel>();


        builder.Services.AddSingleton<AccountService>();
        builder.Services.AddSingleton<LogInService>();
        builder.Services.AddSingleton<PasswordChangingService>();
        builder.Services.AddSingleton<RegistrationService>();
        builder.Services.AddSingleton<BillingAddressService>();
        builder.Services.AddSingleton<ProductsService>();
        builder.Services.AddSingleton<CartService>();
        builder.Services.AddSingleton<OrderService>();
        builder.Services.AddSingleton<FavouritesService>();
        

        return builder.Build();
    }
}
