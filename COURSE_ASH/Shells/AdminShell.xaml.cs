namespace COURSE_ASH.Shells;

public partial class AdminShell : Shell
{
	public AdminShell()
	{
		InitializeComponent();

        BindingContext = new AdminShellViewModel();

        Routing.RegisterRoute(nameof(AccountsPage), typeof(AccountsPage));
        Routing.RegisterRoute(nameof(AddCategoryPage), typeof(AddCategoryPage));
        Routing.RegisterRoute(nameof(AddProductPage), typeof(AddProductPage));
        Routing.RegisterRoute(nameof(AdminSearchPage), typeof(AdminSearchPage));
        Routing.RegisterRoute(nameof(EditCategoryPage), typeof(EditCategoryPage));
        Routing.RegisterRoute(nameof(EditProductPage), typeof(EditProductPage));
        Routing.RegisterRoute(nameof(OrdersCatalogPage), typeof(OrdersCatalogPage));
        Routing.RegisterRoute(nameof(OrderStatusPage), typeof(OrderStatusPage));

        Routing.RegisterRoute(nameof(ProfilePage), typeof(ProfilePage));
        Routing.RegisterRoute(nameof(SearchPage), typeof(SearchPage));
        Routing.RegisterRoute(nameof(ProductPage), typeof(ProductPage));
    }
}