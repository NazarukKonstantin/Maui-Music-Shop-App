namespace COURSE_ASH.View;

public partial class ProductPage : ContentPage
{
    public ProductPage(ProductPageViewModel viewModel)
    {
        InitializeComponent();
        BindingContext=viewModel;
    }
}