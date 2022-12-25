namespace COURSE_ASH.Views.UserViews;

public partial class CatalogPage : ContentPage
{
    public IRefreshable ViewModel { get; set; }

    public CatalogPage(CatalogPageViewModel viewModel)
    {
        InitializeComponent();
        ViewModel=viewModel;
        BindingContext=viewModel;
    }
}

