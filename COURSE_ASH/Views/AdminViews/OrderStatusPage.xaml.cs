namespace COURSE_ASH.Views.AdminViews;

public partial class OrderStatusPage : ContentPage
{
	public OrderStatusPage(OrderStatusPageViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}