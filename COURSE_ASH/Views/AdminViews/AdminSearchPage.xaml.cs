using CommunityToolkit.Maui.Converters;
using Microsoft.Maui.Platform;

namespace COURSE_ASH.Views.AdminViews;

public partial class AdminSearchPage : ContentPage
{
	public AdminSearchPage(AdminSearchPageViewModel viewModel)
	{
		InitializeComponent();

		BindingContext = viewModel;
	}

	private void SearchCategories_SearchButtonPressed(object sender, EventArgs e)
	{
        SearchCategories.Unfocus();
#if ANDROID
        if (Platform.CurrentActivity.CurrentFocus != null)
            Platform.CurrentActivity
                .HideKeyboard(Platform.CurrentActivity.CurrentFocus);
#endif
    }

    private void SearchProducts_SearchButtonPressed(object sender, EventArgs e)
	{
        SearchProducts.Unfocus();
#if ANDROID
        if (Platform.CurrentActivity.CurrentFocus != null)
            Platform.CurrentActivity
                .HideKeyboard(Platform.CurrentActivity.CurrentFocus);
#endif
    }
}