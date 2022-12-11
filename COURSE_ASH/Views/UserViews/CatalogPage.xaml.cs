namespace COURSE_ASH.Views.UserViews;

public partial class CatalogPage : ContentPage
{
    public CatalogPage(CatalogPageViewModel viewModel)
    {
        InitializeComponent();
        BindingContext=viewModel;
    }
}

