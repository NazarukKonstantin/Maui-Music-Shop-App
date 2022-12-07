namespace COURSE_ASH.Views;

public partial class ProductPage : ContentPage
{
    public ProductPage(ProductPageViewModel viewModel)
    {
        InitializeComponent();

        BindingContext = viewModel;
    }
}