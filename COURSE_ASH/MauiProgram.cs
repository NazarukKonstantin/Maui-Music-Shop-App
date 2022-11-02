namespace COURSE_ASH;
using CommunityToolkit.Maui;
using COURSE_ASH.Services;
using COURSE_ASH.View;
using COURSE_ASH.ViewModel;
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

        builder.Services.AddSingleton<MainPage>();
        builder.Services.AddSingleton<CatalogViewModel>();

        builder.Services.AddSingleton<ImageUploadPage>();
        builder.Services.AddSingleton<ImageUploader>();

        builder.Services.AddSingleton<CartPage>();
        builder.Services.AddSingleton<CartPageViewModel>();

        builder.Services.AddTransient<ProductPage>();
        builder.Services.AddTransient<ProductDetailsViewModel>();

        builder.Services.AddSingleton<ProfilePage>();
        builder.Services.AddSingleton<ProfilePageViewModel>();

        builder.Services.AddSingleton<SearchPage>();
        builder.Services.AddSingleton<ProductsViewModel>();

        builder.Services.AddSingleton<LoginPage>();
        builder.Services.AddSingleton<LoginPageViewModel>();

        return builder.Build();
    }
}
