namespace COURSE_ASH.Views.UserViews;

public partial class FavouritesPage : ContentPage
{
	public IRefreshable ViewModel { get; set; } 
	public FavouritesPage(FavouritesPageViewModel viewModel)
	{
		InitializeComponent();
		ViewModel = viewModel;
		BindingContext = viewModel;
	}
}