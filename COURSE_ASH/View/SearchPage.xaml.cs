using COURSE_ASH.ViewModel;

namespace COURSE_ASH.View;

public partial class SearchPage : ContentPage
{
    public SearchPage(SearchPageViewModel viewModel)
    {
        InitializeComponent();
        BindingContext=viewModel;
    }

    //public void ProductSearchHandler_Focused(object sender, EventArgs e)
    //{
    //    Shell.SetTabBarIsVisible(productSearch, !IsFocused);
    //}
}