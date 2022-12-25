namespace COURSE_ASH.Views.UserViews;

public partial class CartPage : ContentPage
{
    public IRefreshable ViewModel { get; set; }
    public CartPage(CartPageViewModel viewModel)
    {
        InitializeComponent();
        ViewModel = viewModel;
        BindingContext = viewModel;
    }
}