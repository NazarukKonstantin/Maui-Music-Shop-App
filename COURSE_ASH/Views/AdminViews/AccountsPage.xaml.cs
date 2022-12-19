namespace COURSE_ASH.Views.AdminViews;

public partial class AccountsPage : ContentPage
{
	public AccountsPage(AccountsPageViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}