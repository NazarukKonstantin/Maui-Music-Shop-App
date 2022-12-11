namespace COURSE_ASH.Views.AdminViews;

public partial class AddProductPage : ContentPage
{
	public AddProductPage(AddProductPageViewModel viewModel)
	{
		InitializeComponent();

		BindingContext = viewModel;
	}
}