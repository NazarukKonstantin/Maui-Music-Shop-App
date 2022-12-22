using Microsoft.Maui.Platform;

namespace COURSE_ASH.Views.AdminViews;

public partial class AddCategoryPage : ContentPage
{
	public AddCategoryPage(AddCategoryPageViewModel viewModel)
	{
		InitializeComponent();

		BindingContext = viewModel;
	}

    private void CategoryNameEntryCompleted(object sender, EventArgs e)
    {
#if ANDROID
        if (Platform.CurrentActivity.CurrentFocus != null)
            Platform.CurrentActivity
                .HideKeyboard(Platform.CurrentActivity.CurrentFocus);
#endif
        CategoryNameEntry.Unfocus();
        imgRotation.Focus();
    }
}