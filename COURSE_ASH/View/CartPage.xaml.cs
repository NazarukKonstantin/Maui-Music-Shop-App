namespace COURSE_ASH.View;

public partial class CartPage : ContentPage
{
    public CartPage(CartPageViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}