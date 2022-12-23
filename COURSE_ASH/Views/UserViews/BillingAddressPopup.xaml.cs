namespace COURSE_ASH.Views.UserViews;

public partial class BillingAddressPopup : Popup
{
	public BillingAddressPopup(BillingAddressPopupViewModel viewModel)
	{
		InitializeComponent();
        BindingContext = viewModel;
	}

    private void Picker_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (((Picker)sender).SelectedItem is not null)
        {
            addressFields.IsVisible = true;
        }
        else
        {
            addressFields.IsVisible = false;
            cityEntry.Text = string.Empty;
            streetEntry.Text = string.Empty;
            buildingEntry.Text = string.Empty;
            apartmentEntry.Text = string.Empty;
            postalcodeEntry.Text = string.Empty;
        }
    }
    private void ConfirmButton_Clicked(object sender, EventArgs e)
    {
        Close();
    }
}