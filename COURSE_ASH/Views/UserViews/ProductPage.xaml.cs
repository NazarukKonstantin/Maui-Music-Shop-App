namespace COURSE_ASH.Views.UserViews;

public partial class ProductPage : ContentPage
{
    public ProductPage(ProductPageViewModel viewModel)
    {
        InitializeComponent();

        BindingContext = viewModel;
    }
}