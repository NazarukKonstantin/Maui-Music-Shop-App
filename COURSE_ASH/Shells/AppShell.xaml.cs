namespace COURSE_ASH;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();

        BindingContext = new AppShellViewModel();

        Routing.RegisterRoute(nameof(BillingAddressPage), typeof(BillingAddressPage));
        Routing.RegisterRoute(nameof(CartPage), typeof(CartPage));
        Routing.RegisterRoute(nameof(CatalogPage), typeof(CatalogPage));
        Routing.RegisterRoute(nameof(CheckoutPage), typeof(CheckoutPage));
        Routing.RegisterRoute(nameof(FavouritesPage), typeof(FavouritesPage));
        Routing.RegisterRoute(nameof(OrderHistoryPage), typeof(OrderHistoryPage));
        Routing.RegisterRoute(nameof(OrderPage), typeof(OrderPage));

        Routing.RegisterRoute(nameof(ProductPage), typeof(ProductPage));
        Routing.RegisterRoute(nameof(SearchPage), typeof(SearchPage));
        Routing.RegisterRoute(nameof(ProfilePage), typeof(ProfilePage));
    }
}
