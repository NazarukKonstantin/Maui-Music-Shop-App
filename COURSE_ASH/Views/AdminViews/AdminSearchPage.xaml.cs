namespace COURSE_ASH.Views.AdminViews;

public partial class AdminSearchPage : ContentPage
{
	public AdminSearchPage(AdminSearchPageViewModel viewModel)
	{
		InitializeComponent();

		BindingContext = viewModel;
	}
}