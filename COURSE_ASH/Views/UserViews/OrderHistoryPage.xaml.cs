namespace COURSE_ASH.Views.UserViews;

public partial class OrderHistoryPage : ContentPage
{
	public OrderHistoryPage(OrderHistoryPageViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}