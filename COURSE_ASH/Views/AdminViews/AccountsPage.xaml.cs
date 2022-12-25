namespace COURSE_ASH.Views.AdminViews;

public partial class AccountsPage : ContentPage
{
	public IRefreshable ViewModel { get; set; }
	public AccountsPage(AccountsPageViewModel viewModel)
	{
		InitializeComponent();
		ViewModel = viewModel;
		BindingContext = viewModel;
	}
}