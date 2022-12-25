namespace COURSE_ASH.Shells;

public partial class AdminShell : Shell
{
	public AdminShell()
	{
		InitializeComponent();

        Routing.RegisterRoute(nameof(AccountsPage), typeof(AccountsPage));
        Routing.RegisterRoute(nameof(AddCategoryPage), typeof(AddCategoryPage));
        Routing.RegisterRoute(nameof(AddProductPage), typeof(AddProductPage));
        Routing.RegisterRoute(nameof(AdminSearchPage), typeof(AdminSearchPage));
        Routing.RegisterRoute(nameof(EditCategoryPage), typeof(EditCategoryPage));
        Routing.RegisterRoute(nameof(EditProductPage), typeof(EditProductPage));
        Routing.RegisterRoute(nameof(OrdersCatalogPage), typeof(OrdersCatalogPage));

        Routing.RegisterRoute(nameof(ProfilePage), typeof(ProfilePage));
        Routing.RegisterRoute(nameof(SearchPage), typeof(SearchPage));
        Routing.RegisterRoute(nameof(ProductPage), typeof(ProductPage));
    }

    protected override void OnNavigated(ShellNavigatedEventArgs args)
    {
        string location = args.Current?.Location?.ToString();
        switch (location)
        {
            case $"//{nameof(AdminSearchPage)}":
                ((AdminSearchPage)Shell.Current.CurrentPage).ViewModel.RefreshAsync();
                break;
            case $"//{nameof(AccountsPage)}":
                ((AccountsPage)Shell.Current.CurrentPage).ViewModel.RefreshAsync();
                break;
            case $"//{nameof(OrdersCatalogPage)}":
                ((OrdersCatalogPage)Shell.Current.CurrentPage).ViewModel.RefreshAsync();
                break;
            default: break;
        }

        base.OnNavigated(args);
    }
}