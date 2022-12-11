namespace COURSE_ASH.Views.UserViews;

public partial class CartPage : ContentPage
{
    public CartPage(CartPageViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}