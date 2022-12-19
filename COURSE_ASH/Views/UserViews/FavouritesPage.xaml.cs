namespace COURSE_ASH.Views.UserViews;

public partial class FavouritesPage : ContentPage
{
	public FavouritesPage(FavouritesPageViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}