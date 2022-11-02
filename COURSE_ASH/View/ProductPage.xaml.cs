
using COURSE_ASH.ViewModel;

namespace COURSE_ASH.View;

public partial class ProductPage : ContentPage
{
    public ProductPage(ProductDetailsViewModel viewModel)
    {
        InitializeComponent();
        BindingContext=viewModel;
    }

    public async void ScrollView_Scrolled(object sender, ScrolledEventArgs e)
    {
        ToCartButton.IsVisible = false;
        await Task.Delay(1000);
        ToCartButton.IsVisible = true;
    }
}