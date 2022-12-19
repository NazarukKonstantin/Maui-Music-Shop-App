namespace COURSE_ASH.Views.AdminViews;

public partial class EditCategoryPage : ContentPage
{
	public EditCategoryPage(EditCategoryPageViewModel viewModel)
	{
		InitializeComponent();

		BindingContext = viewModel;
	}
}