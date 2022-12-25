namespace COURSE_ASH.Views.UserViews;

public partial class OrderPage : ContentPage
{
	public OrderPage(OrderPageViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}