namespace COURSE_ASH.Views.AdminViews;

public partial class EditProductPage : ContentPage
{
	public EditProductPage(EditProductPageViewModel viewModel)
	{
		InitializeComponent();
		BindingContext=viewModel;
	}
}