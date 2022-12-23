namespace COURSE_ASH.Views.AdminViews;

public partial class OrderStatusPopup : Popup
{
	public OrderStatusPopup(OrderStatusPopupViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}

	private void UpdateStatusButton_Clicked(object sender, EventArgs e)
	{
		Close();
	}
}