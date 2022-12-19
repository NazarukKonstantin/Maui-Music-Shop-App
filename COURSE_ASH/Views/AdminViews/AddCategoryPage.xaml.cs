namespace COURSE_ASH.Views.AdminViews;

public partial class AddCategoryPage : ContentPage
{
	public AddCategoryPage(AddCategoryPageViewModel viewModel)
	{
		InitializeComponent();

		BindingContext = viewModel;
	}
}