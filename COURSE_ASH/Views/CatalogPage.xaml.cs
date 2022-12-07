namespace COURSE_ASH.Views;

public partial class CatalogPage : ContentPage
{
    public CatalogPage(CatalogPageViewModel viewModel)
    {
        InitializeComponent();
        BindingContext=viewModel;
    }
}

