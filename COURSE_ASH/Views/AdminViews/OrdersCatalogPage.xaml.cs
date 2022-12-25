using COURSE_ASH.Models;

namespace COURSE_ASH.Views.AdminViews;

public partial class OrdersCatalogPage : ContentPage
{
   public  IRefreshable ViewModel { get; set; }
	public OrdersCatalogPage (OrdersCatalogPageViewModel viewModel)
	{
		InitializeComponent();
        ViewModel = viewModel;
		BindingContext = viewModel;
	}

    [RelayCommand]
    private void PopUp(Order order)
    {
        this.ShowPopup(new OrderStatusPopup(
            new OrderStatusPopupViewModel(new OrderService(), order)));
    }
}