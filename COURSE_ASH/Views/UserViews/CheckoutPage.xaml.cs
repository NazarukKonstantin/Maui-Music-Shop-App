using COURSE_ASH.Models;

namespace COURSE_ASH.Views.UserViews;

public partial class CheckoutPage : ContentPage
{
	public CheckoutPage()
	{
		InitializeComponent();
	}

    private void EnterAddress_Clicked(object sender, EventArgs e)
    {
        this.ShowPopup(new BillingAddressPopup(
                new BillingAddressPopupViewModel(new BillingAddressService())));
    }
}