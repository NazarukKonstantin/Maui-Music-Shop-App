using COURSE_ASH.Models;

namespace COURSE_ASH.Views.UserViews;

public partial class CheckoutPage : ContentPage
{
	public CheckoutPage(CheckoutPageViewModel viewModel)
	{
		InitializeComponent();
        BindingContext = viewModel;
	}

    private void EnterAddress_Clicked(object sender, EventArgs e)
    {
        this.ShowPopup(new BillingAddressPopup(
                new BillingAddressPopupViewModel(new BillingAddressService())));
    }
}