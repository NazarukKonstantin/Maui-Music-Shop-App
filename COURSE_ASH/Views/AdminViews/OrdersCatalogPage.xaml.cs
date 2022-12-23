using COURSE_ASH.Models;

namespace COURSE_ASH.Views.AdminViews;

public partial class OrdersCatalogPage : ContentPage
{
	public OrdersCatalogPage (OrdersCatalogPageViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}

    [RelayCommand]
    private void PopUp(Order order)
    {
        this.ShowPopup(new OrderStatusPopup(
            new OrderStatusPopupViewModel(new OrderService(), order)));
    }
}