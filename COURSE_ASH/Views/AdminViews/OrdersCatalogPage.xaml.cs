namespace COURSE_ASH.Views.AdminViews;

public partial class OrdersCatalogPage : ContentPage
{
	public OrdersCatalogPage (OrdersCatalogPageViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}