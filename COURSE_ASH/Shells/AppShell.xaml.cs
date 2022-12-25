namespace COURSE_ASH;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();

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

    protected override void OnNavigated(ShellNavigatedEventArgs args)
    {
        string location = args.Current?.Location?.ToString();
        switch (location)
        {
            case $"//{nameof(CatalogPage)}":
                ((CatalogPage)Shell.Current.CurrentPage).ViewModel.RefreshAsync();
                break;
            case $"//{nameof(CartPage)}":
                ((CartPage)Shell.Current.CurrentPage).ViewModel.RefreshAsync();
                break;
            case $"//{nameof(FavouritesPage)}":
                ((FavouritesPage)Shell.Current.CurrentPage).ViewModel.RefreshAsync();
                break;
            case $"//{nameof(SearchPage)}":
                ((SearchPage)Shell.Current.CurrentPage).ViewModel.RefreshAsync();
                break;
            default: break;
        }

        base.OnNavigated(args);
    }
}
