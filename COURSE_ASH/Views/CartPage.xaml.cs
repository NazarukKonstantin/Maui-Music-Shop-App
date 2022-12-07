namespace COURSE_ASH.Views;

public partial class CartPage : ContentPage
{
    public CartPage(CartPageViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}