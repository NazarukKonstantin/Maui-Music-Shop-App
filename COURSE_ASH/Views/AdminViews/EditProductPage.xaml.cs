using Microsoft.Maui.Platform;

namespace COURSE_ASH.Views.AdminViews;

public partial class EditProductPage : ContentPage
{
	public EditProductPage(EditProductPageViewModel viewModel)
	{
		InitializeComponent();
		BindingContext=viewModel;
	}

    private void ModelEntry_Completed(object sender, EventArgs e)
    {
        ModelEntry.Unfocus();
        PriceEntry.Focus();
    }

    private void PriceEntry_Completed(object sender, EventArgs e)
    {
        PriceEntry.Unfocus();
        CategoryPicker.Focus();
    }


    private void CategoryPicker_SelectedIndexChanged(object sender, EventArgs e)
    {
        CategoryPicker.Unfocus();
        ProductTypeEntry.Focus();
    }

    //private void CategoryEntry_Completed(object sender, EventArgs e)
    //{
    //	CategoryEntry.Unfocus();
    //	ProductTypeEntry.Focus();
    //}

    private void ProductTypeEntry_Completed(object sender, EventArgs e)
    {
        ProductTypeEntry.Unfocus();
        InfoEditor.Focus();
    }

    private void InfoEditor_Completed(object sender, EventArgs e)
    {
        InfoEditor.Unfocus();
#if ANDROID
        if (Platform.CurrentActivity.CurrentFocus != null)
            Platform.CurrentActivity
                .HideKeyboard(Platform.CurrentActivity.CurrentFocus);
#endif
    }
}