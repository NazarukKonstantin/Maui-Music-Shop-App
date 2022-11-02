using COURSE_ASH.ViewModel;
using CommunityToolkit.Maui;

namespace COURSE_ASH.View;

public partial class MainPage : ContentPage
{
    public MainPage(CatalogViewModel viewModel)
    {
        InitializeComponent();
        BindingContext=viewModel;
    }
}

